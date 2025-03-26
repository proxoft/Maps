using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;
using Proxoft.Optional;

namespace Proxoft.Maps.OpenStreetMap.Geocoding;

public sealed class OsmGeocoder : IGeocoder, IDisposable
{
    private readonly ConsoleLogger _logger;
    private readonly IOsmResultParser _parser;

    private readonly string _language;

    private readonly HttpClient _http = new()
    {
        BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
    };

    public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
    {
        Console.WriteLine($"OsmGeocoder logging to exceptions: {options.ConsoleLogExceptions}");
        Console.WriteLine($"OsmGeocoder tracing to console: {options.ConsoleTraceLogGeocoder}");

        _logger = new ConsoleLogger(options.ConsoleTraceLogGeocoder, options.ConsoleLogExceptions);

        _parser = parser;
        _language = options.Language;
    }

    public ApiStatus Status => ApiStatus.Available;

    public async Task<Either<ErrorStatus, Address>> Geocode(string location)
    {
        GeocodeResult[] results = await _http.Geocode(location, _language, _logger);
        return _parser.Parse(results);
    }

    public async Task<Either<ErrorStatus, Address>> Geocode(string city, string? street = null, string? streetNumber = null, string? country = null)
    {
        AddressSearch addressSearch = new()
        {
            City = city,
            Street = street,
            StreetNumber = streetNumber,
            Country = country
        };

        GeocodeResult[] results = await _http.Geocode(addressSearch, _language, _logger);
        foreach (GeocodeResult result in results)
        {
            Console.WriteLine(result);
        }
        return _parser.Parse(results);
    }

    public Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng)
        => this.Geocode(latLng.Latitude, latLng.Longitude);

    public async Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
    {
        GeocodeResult[] results = await _http.Geocode(latitude, longitude, _language, _logger); 
        return _parser.Parse(results);
    }

    public async Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string location)
    {
        GeocodeResult[] geocodeResults = [.. await _http.GeocodeStreet(location, _logger)];
        Either<ErrorStatus, StreetGeometry> either =await this.FindStreetGeometry(geocodeResults);
        return either;
    }

    public async Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string city, string streetName)
    {
        GeocodeResult[] geocodeResults = [.. await _http.GeocodeStreet(new AddressSearch { City = city, Street = streetName }, _logger)];
        Either<ErrorStatus, StreetGeometry> either = await this.FindStreetGeometry(geocodeResults);
        return either;
    }

    public void Dispose()
    {
        _http.Dispose();
    }

    private async Task<Either<ErrorStatus, StreetGeometry>> FindStreetGeometry(GeocodeResult[] geocodeResults)
    {
        Task<StreetResult>[] t = [
           ..geocodeResults
                .Where(r => r.place_rank is 26 or 27)
                .Select(r => r.osm_id)
                .Select(osmId => _http.GetStreetDetail(osmId, _logger))
       ];

        StreetResult[] streetResults = await Task.WhenAll(t);
        Either<ErrorStatus, StreetGeometry> either = _parser.Parse(streetResults);
        return either;
    }
}

file record AddressSearch
{
    public string? City { get; init; }

    public string? Street { get; init; }

    public string? StreetNumber { get; init; }

    public string? Country { get; init; }
}


file static class StreetGeometryOperators
{
    private static readonly string[] _streetGeometryParameters = [
        "osmtype=W",
        "format=json",
        "polygon_geojson=1"
    ];

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        string location,
        ConsoleLogger logger)
    {
        string path = "search".ToQueryPath(
            [
                .._streetGeometryParameters,
                $"q={Uri.EscapeDataString(location)}"
            ]
        );

        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        AddressSearch addressSearch,
        ConsoleLogger logger)
    {
        string[] parameters = [
            .._streetGeometryParameters,
            .. addressSearch.ToSearchParameters()
        ];

        string path = "search".ToQueryPath(parameters);
        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }

    public static async Task<StreetResult> GetStreetDetail(
        this HttpClient http,
        long osmId,
        ConsoleLogger logger)
    {
        string path ="details".ToQueryPath(
            [
            .._streetGeometryParameters,
            $"osmid={osmId}"
            ]
        );

        StreetResult result = await http.GetFrom<StreetResult>(path, () => new(), logger);
        return result;
    }
}

file static class GeocodingOperators
{
    private static readonly string[] _geocodeParameters = [
        "addressdetails=1",
        "format=json"
    ];

    public static Task<GeocodeResult[]> Geocode(this HttpClient http, string location, string language, ConsoleLogger logger)
    {
        string[] parameters = [
            .._geocodeParameters,
            .. language.AcceptLanguage(),
            $"q={Uri.EscapeDataString(location)}"
        ];

        string path = "search".ToQueryPath(parameters);
        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }

    public static Task<GeocodeResult[]> Geocode(this HttpClient http, AddressSearch addressSearch, string language, ConsoleLogger logger)
    {
        string[] parameters = [
            .._geocodeParameters,
            ..addressSearch.ToSearchParameters(),
            ..language.AcceptLanguage(),
        ];

        string path = "search".ToQueryPath(parameters);
        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }

    public static async Task<GeocodeResult[]> Geocode(this HttpClient http, decimal latitude, decimal longitude, string language, ConsoleLogger logger)
    {
        string[] parameters = [
            .. _geocodeParameters,
            ..language.AcceptLanguage(),
            $"lat={latitude.ToString(CultureInfo.InvariantCulture)}",
            $"lon={longitude.ToString(CultureInfo.InvariantCulture)}"
        ];

        string path = "reverse".ToQueryPath(parameters);
        GeocodeResult? result =  await http.GetFrom<GeocodeResult?>(path, () => null, logger);
        return result is null
            ? []
            : [result];
    }

    private static IEnumerable<string> AcceptLanguage(this string language) =>
        string.IsNullOrEmpty(language) ? [] : [$"accept-language={language}"];
}

file static class HttpExtensions
{
    public static async Task<T> GetFrom<T>(
        this HttpClient http,
        string queryPath,
        Func<T> fallback,
        ConsoleLogger logger)
    {
        logger.LogMessage($"querying: {queryPath}");

        try
        {
            T response = await http.GetFromJsonAsync<T>(queryPath) ?? fallback();
            return response;
        }
        catch (Exception ex)
        {
            logger.LogException(ex, "Error GetFrom:");
            return fallback();
        }
    }


    public static string ToQueryPath(this string path, IEnumerable<string> parameters) =>
        $"{path}?{parameters.ToQueryParameters()}";

    public static IEnumerable<string> ToSearchParameters(this AddressSearch search)
    {
        if (!string.IsNullOrWhiteSpace(search.City))
        {
            yield return $"city={Uri.EscapeDataString(search.City)}";
        }

        if (!string.IsNullOrWhiteSpace(search.Street) || !string.IsNullOrWhiteSpace(search.StreetNumber))
        {
            string?[] parts = [
                search.Street,
                search.StreetNumber
            ];

            yield return $"street={Uri.EscapeDataString(string.Join(" ", parts.Where(s => !string.IsNullOrWhiteSpace(s))))}";
        }

        if (!string.IsNullOrWhiteSpace(search.Country))
        {
            yield return $"country={search.Country}";
        }
    }

    private static string ToQueryParameters(this IEnumerable<string> items) =>
        string.Join("&", items);
}