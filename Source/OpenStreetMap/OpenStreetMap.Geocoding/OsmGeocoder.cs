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
    private readonly OpenStreetMapOptions _options;
    private readonly IOsmResultParser _parser;
    private readonly string _geocodeParameters;
    private readonly string _streetGeometryParameters;

    private readonly HttpClient _http = new()
    {
        BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
    };

    public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
    {
        _options = options;

        Console.WriteLine($"Logging to console: {_options.ConsoleLogExceptions}");

        _parser = parser;
        _geocodeParameters = $"addressdetails=1&format=json&limit=1&accept-language={options.Language}";
        _streetGeometryParameters = $"osmtype=W&format=json";
    }

    public ApiStatus Status => ApiStatus.Available;

    public async Task<Either<ErrorStatus, Address>> Geocode(string location)
    {
        try
        {
            string path = $"search?{_geocodeParameters}&q={location}";
            this.ConsoleLog($"searching: {path}");

            Result[]? response = await _http.GetFromJsonAsync<Result[]>(path);

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

            Result[]? response = await _http.GetFromJsonAsync<Result[]>(path);
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

            Result? response = await _http.GetFromJsonAsync<Result>(path);
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
        Result[] response = [];
        try
        {
            response = await _http.GetFromJsonAsync<Result[]>($"search?{_streetGeometryParameters}&q={location}") ?? [];
        }
        catch (Exception ex)
        {
            this.ConsoleLogException(ex);
            return ErrorStatus.UnknownError;
        }

        Task<StreetLine>[] tasks = [
            ..response
                .Where(r => r.place_rank == 26 || r.place_rank == 27)
                .Select(r => r.osm_id)
                .Select(osmId => _http.FindStreetCoordinates(osmId, this.ConsoleLogException))
        ];

        StreetLine[] lines = await Task.WhenAll(tasks);

        return new StreetGeometry()
        {
            Lines = lines
        };
    }

    public Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string city, string streetName)
    {
        throw new NotImplementedException();
    }

    public Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(decimal latitude, decimal longitude)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _http.Dispose();
    }

    private Either<ErrorStatus, Address> ParseResults(Result[] results) =>
        results.Length == 0
        ? ErrorStatus.ZeroResults
        : this.ParseResult(results[0]);

    private Either<ErrorStatus, Address> ParseResult(Result result)
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

file static class HttpExtensions
{
    private const string _streetGeometryParameters = $"osmtype=W&format=json&polygon_geojson=1";

    public static async Task<StreetLine> FindStreetCoordinates(
        this HttpClient http,
        long osmId,
        Action<Exception> exceptionLog)
    {
        try
        {
            string path = $"details?{_streetGeometryParameters}&osmid={osmId}";
            StreetResult result = await http.GetFromJsonAsync<StreetResult>(path) ?? new();

            LatLng[] latLngs = [..
                result.geometry.coordinates
                .Select(c => new LatLng() { Longitude = c[0], Latitude = c[1] })
            ];

            return new StreetLine(latLngs);
        }
        catch (Exception ex)
        {
            exceptionLog(ex);
            return StreetLine.Empty;
        }
    }
}

file static class ParsingExtensions
{
    public static StreetGeometry Parse(this StreetResult street) =>
        new()
        {
            Coordinates = [..
                street.geometry.coordinates
                   .Select(c => new LatLng() { Longitude = c[0], Latitude = c[1] })
            ]
        };
    
}