using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

    private const int _streetSearchLimitMin = 1;
    private const int _streetSearchLimitMax = 20;

    private readonly int _streetSearchLimit;

    private readonly string _language;
    private readonly HttpClient _http;

    public OsmGeocoder(HttpClient httpClient, OpenStreetMapOptions options, IOsmResultParser parser)
    {
        Console.WriteLine($"OsmGeocoder logging to exceptions: {options.ConsoleLogExceptions}");
        Console.WriteLine($"OsmGeocoder tracing to console: {options.ConsoleTraceLogGeocoder}");

        _logger = new ConsoleLogger(options.ConsoleTraceLogGeocoder, options.ConsoleLogExceptions);
        _http = httpClient;
        _parser = parser;
        _language = options.Language;
        _streetSearchLimit = Math.Max(_streetSearchLimitMin, Math.Min(_streetSearchLimitMax, options.StreetGeometryMaxIterations)); // ensure the number to be between 1 and 20
    }

    public ApiStatus Status => ApiStatus.Available;

    public async Task<Either<ErrorStatus, Address>> Geocode(string location)
    {
        GeocodeResult[] results = await _http.Geocode(location, _language, _logger);
        return _parser.ParseAddress(results);
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
        return _parser.ParseAddress(results);
    }

    public Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng)
        => this.Geocode(latLng.Latitude, latLng.Longitude);

    public async Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
    {
        GeocodeResult[] results = await _http.Geocode(latitude, longitude, _language, _logger); 
        return _parser.ParseAddress(results);
    }

    public async Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string location)
    {
        Either<ErrorStatus, StreetGeometry> either = await this.Geocode(location)
            .Map(address => this.GeocodeStreet(address.City, address.Street));

        return either;
    }

    public async Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string city, string streetName)
    {
        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(streetName))
        {
            return ErrorStatus.ZeroResults;
        }

        GeocodeResult[] geocodeResults = [];
        int iteration = 0;
        do
        {
            iteration++;
            _logger.LogMessage($"GeocodeStreet {iteration}/{_streetSearchLimit}..");

            long[] excludePlaceIds = [.. geocodeResults.Select(r => r.place_id)];
            GeocodeResult[] results = [.. await _http.GeocodeStreet(new AddressSearch { City = city, Street = streetName }, excludePlaceIds, _logger)];
            geocodeResults = [
                ..geocodeResults,
                ..results
            ];

            if(results.Length == 0 || iteration >= _streetSearchLimit)
            {
                break;
            }
        } while (true);

        Either<ErrorStatus, StreetGeometry> either = _parser.Parse(geocodeResults);
        return either;
    }

    public void Dispose()
    {
        _http.Dispose();
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
        "limit=40",
        "polygon_geojson=1"
    ];

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        string location,
        IEnumerable<long> excludePlaceIds,
        ConsoleLogger logger)
    {
        string path = "search".ToQueryPath(
            [
                .._streetGeometryParameters,
                $"q={Uri.EscapeDataString(location)}",
                excludePlaceIds.ToQueryParameter()
            ]
        );

        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        AddressSearch addressSearch,
        IReadOnlyCollection<long> excludePlaceIds,
        ConsoleLogger logger)
    {
        string[] parameters = [
            .._streetGeometryParameters,
            .. addressSearch.ToSearchParameters(),
            excludePlaceIds.ToQueryParameter()
        ];

        string path = "search".ToQueryPath(parameters);
        return http.GetFrom<GeocodeResult[]>(path, () => [], logger);
    }
}

file static class GeocodingOperators
{
    private static readonly string[] _geocodeParameters = [
        "addressdetails=1",
        "limit=1",
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
            HttpResponseMessage message = await http.GetAsync(queryPath);
            string content = await message.Content.ReadAsStringAsync();
            logger.LogMessage(content);

            T response = JsonSerializer.Deserialize<T>(content) ?? fallback();
            // T response = await http.GetFromJsonAsync<T>(queryPath) ?? fallback();
            // logger.LogMessage(JsonSerializer.Serialize(response));

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

    public static string ToQueryParameter(this IEnumerable<long> excludePlaceIds)
    {
        string ids = string.Join(",", excludePlaceIds);
        return string.IsNullOrWhiteSpace(ids)
            ? ""
            : $"exclude_place_ids={Uri.EscapeDataString(ids)}";
    }

    private static string ToQueryParameters(this IEnumerable<string> items) =>
        string.Join("&", items.Where(i => !string.IsNullOrWhiteSpace(i)));
}