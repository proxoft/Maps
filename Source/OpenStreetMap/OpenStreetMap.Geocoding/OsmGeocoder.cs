using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding;

public sealed class OsmGeocoder : IGeocoder, IDisposable
{
    private readonly OpenStreetMapOptions _options;
    private readonly IOsmResultParser _parser;
    private readonly string _resultParameters;

    private readonly HttpClient _http = new()
    {
        BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
    };

    public ApiStatus Status => ApiStatus.Available;

    public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
    {
        _options = options;
        Console.WriteLine($"Logging to console: {_options.ConsoleLogExceptions}");
        _parser = parser;
        _resultParameters = $"addressdetails=1&format=json&limit=1&accept-language={options.Language}";
    }

    public async Task<Either<ErrorStatus, Address>> Geocode(string location)
    {
        try
        {
            Result[]? response = await _http.GetFromJsonAsync<Result[]>($"search?{_resultParameters}&q={location}");

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
            Result[]? response = await _http.GetFromJsonAsync<Result[]>($"search?{_resultParameters}&{searchParameters}");
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
            Result? response = await _http.GetFromJsonAsync<Result>(FormattableString.Invariant($"reverse?{_resultParameters}&lat={latitude}&lon={longitude}"));
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

    public void Dispose()
    {
        _http.Dispose();
    }

    private Either<ErrorStatus, Address> ParseResults(Result[] results)
    {
        try
        {
            if (results.Length == 0)
            {
                return ErrorStatus.ZeroResults;
            }

            var address = this.ParseResult(results[0]);
            return address;
        }
        catch (Exception ex)
        {
            this.ConsoleLogException(ex);
            return ErrorStatus.UnknownError;
        }
    }

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

    private void ConsoleLogException(Exception exception)
    {
        if (!_options.ConsoleLogExceptions)
        {
            return;
        }

        Console.WriteLine(exception);
    }
}
