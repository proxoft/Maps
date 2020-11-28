using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Geocoding;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding
{
    public sealed class OsmGeocoder : IGeocoder, IDisposable
    {
        private readonly IOsmResultParser _parser;
        private readonly string _resultParameters;

        private readonly HttpClient _http = new()
        {
            BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
        };

        public ApiStatus Status => ApiStatus.Available;

        public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
        {
            _parser = parser;
            _resultParameters = $"addressdetails=1&format=json&limit=1&accept-language={options.Language}";
        }

        public async Task<Either<ErrorStatus, Core.Geocoding.Address>> Geocode(string location)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<Result[]>($"search?{_resultParameters}&q={location}");
                return this.ParseResults(response);
            }
            catch
            {
                return ErrorStatus.UnknownError;
            }
        }

        public async Task<Either<ErrorStatus, LatLng>> Geocode(Core.Geocoding.Address address)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<Result[]>($"search?{_resultParameters}&city={address.City}");
                return ErrorStatus.UnknownError;
            }
            catch
            {
                return ErrorStatus.UnknownError;
            }
        }

        public Task<Either<ErrorStatus, Core.Geocoding.Address>> Geocode(LatLng latLng)
            => this.Geocode(latLng.Latitude, latLng.Longitude);

        public async Task<Either<ErrorStatus, Core.Geocoding.Address>> Geocode(decimal latitude, decimal longitude)
        {
            try
            {
                var json = await _http.GetStringAsync($"reverse?{_resultParameters}&lat={latitude}&lon={longitude}");
                Console.WriteLine(json);
                var response = await _http.GetFromJsonAsync<Result>($"reverse?{_resultParameters}&lat={latitude}&lon={longitude}");
                return this.ParseResult(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return ErrorStatus.UnknownError;
            }
        }

        private Either<ErrorStatus, Core.Geocoding.Address> ParseResults(Result[] results)
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
            catch
            {
                return ErrorStatus.UnknownError;
            }
        }

        private Either<ErrorStatus, Core.Geocoding.Address> ParseResult(Result result)
        {
            try
            {
                var address = _parser.Parse(result);
                return address;
            }
            catch
            {
                return ErrorStatus.UnknownError;
            }
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
