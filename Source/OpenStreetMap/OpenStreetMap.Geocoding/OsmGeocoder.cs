using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    private readonly OpenStreetMapOptions _options;
    private readonly IOsmResultParser _parser;
    private readonly string _geocodeParameters;

    private readonly HttpClient _http = new()
    {
        BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
    };

    public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
    {
        Console.WriteLine($"OsmGeocoder logging to exceptions: {options.ConsoleLogExceptions}");
        Console.WriteLine($"OsmGeocoder tracing to console: {options.ConsoleTraceLogGeocoder}");

        _logger = new ConsoleLogger(options.ConsoleTraceLogGeocoder, options.ConsoleLogExceptions);
        _options = options;

        _parser = parser;
        _geocodeParameters = $"addressdetails=1&format=json&limit=1&accept-language={options.Language}";
    }

    public ApiStatus Status => ApiStatus.Available;

    public async Task<Either<ErrorStatus, Address>> Geocode(string location)
    {
        try
        {
            string path = $"search?{_geocodeParameters}&q={location}";
            this.ConsoleLog($"searching: {path}");

            GeocodeResult[]? response = await _http.GetFromJsonAsync<GeocodeResult[]>(path);

            return response is null
                ? ErrorStatus.UnknownError
                : this.ParseResults(response);
        }
        catch(Exception ex)
        {
            this.ConsoleLogException(ex);
            return ErrorStatus.UnknownError;
        }
    }

    public async Task<Either<ErrorStatus, Address>> Geocode(string city, string? street = null, string? streetNumber = null, string? country = null)
    {
        var searchParameters = string.Join("&", SearchParameters(city, street, streetNumber, country));
        try
        {
            string path = $"search?{_geocodeParameters}&{searchParameters}";
            this.ConsoleLog($"searching: {path}");

            GeocodeResult[]? response = await _http.GetFromJsonAsync<GeocodeResult[]>(path);
            var maybe = response is null
                ? ErrorStatus.UnknownError
                : this.ParseResults(response);

            return maybe;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return ErrorStatus.UnknownError;
        }
    }

    public Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng)
        => this.Geocode(latLng.Latitude, latLng.Longitude);

    public async Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
    {
        try
        {
            string path = FormattableString.Invariant($"reverse?{_geocodeParameters}&lat={latitude}&lon={longitude}");
            this.ConsoleLog($"searching: {path}");

            GeocodeResult? response = await _http.GetFromJsonAsync<GeocodeResult>(path);
            return response is null
                ? ErrorStatus.UnknownError
                : this.ParseResult(response);
        }
        catch(Exception ex)
        {
            this.ConsoleLogException(ex);
            return ErrorStatus.UnknownError;
        }
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

    private Either<ErrorStatus, Address> ParseResults(GeocodeResult[] results) =>
        results.Length == 0
        ? ErrorStatus.ZeroResults
        : this.ParseResult(results[0]);

    private Either<ErrorStatus, Address> ParseResult(GeocodeResult result)
    {
        try
        {
            var address = _parser.Parse(result);
            return address;
        }
        catch (Exception ex)
        {
            this.ConsoleLogException(ex);
            return ErrorStatus.UnknownError;
        }
    }

    private static IEnumerable<string> SearchParameters(string city, string? street, string? streetNumber, string? country)
    {
        yield return $"city={city}";

        if (!string.IsNullOrWhiteSpace(street) || !string.IsNullOrWhiteSpace(streetNumber))
        {
            yield return $"street={streetNumber} {street}";
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            yield return $"country={country}";
        }
    }

    private void ConsoleLog(string message)
    {
        if (!_options.ConsoleLogExceptions)
        {
            return;
        }

        Console.WriteLine(message);
    }

    private void ConsoleLogException(Exception exception)
    {
        if (!_options.ConsoleLogExceptions)
        {
            return;
        }

        Console.WriteLine(exception);
    }
}

file class AddressSearch
{
    public string? City { get; set; }

    public string? Street { get; set; }

    public string? StreetNumber { get; set; }

    public string? Country { get; set; }
}

file static class StreetGeometryOperators
{
    private const string _streetGeometryParameters = $"osmtype=W&format=json&polygon_geojson=1";

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        string location,
        ConsoleLogger logger) =>
        http.GetFrom<GeocodeResult[]>($"search?{_streetGeometryParameters}&q={location}", () => [], logger);

    public static Task<GeocodeResult[]> GeocodeStreet(
        this HttpClient http,
        AddressSearch addressSearch,
        ConsoleLogger logger)
    {
        string[] parameters = [_streetGeometryParameters, .. addressSearch.ToSearchParameters()];
        string sp = string.Join("&", parameters);
        return http.GetFrom<GeocodeResult[]>($"search?{sp}", () => [], logger);
    }

    public static async Task<StreetResult> GetStreetDetail(
        this HttpClient http,
        long osmId,
       ConsoleLogger logger)
    {
        string path = $"details?{_streetGeometryParameters}&osmid={osmId}";
        StreetResult result = await http.GetFrom<StreetResult>(path, () => new(), logger);
        return result;
    }
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

    public static IEnumerable<string> ToSearchParameters(this AddressSearch search)
    {
        if (!string.IsNullOrWhiteSpace(search.City))
        {
            yield return $"city={search.City}";
        }

        if (!string.IsNullOrWhiteSpace(search.Street) || !string.IsNullOrWhiteSpace(search.StreetNumber))
        {
            string?[] parts = [
                search.Street,
                search.StreetNumber
            ];

            yield return $"street={string.Join(" ", parts.Where(s => !string.IsNullOrWhiteSpace(s)))}";
        }

        if (!string.IsNullOrWhiteSpace(search.Country))
        {
            yield return $"country={search.Country}";
        }
    }
}