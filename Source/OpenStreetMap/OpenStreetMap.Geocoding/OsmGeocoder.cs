using System;
using System.Net.Http;
using System.Text.Json;
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
        private readonly OpenStreetMapOptions _options;
        private readonly IOsmResultParser _parser;
        private readonly HttpClient _http = new()
        {
            BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
        };

        public OsmGeocoder(OpenStreetMapOptions options, IOsmResultParser parser)
        {
            _options = options;
            _parser = parser;
        }

        public async Task<Either<ErrorStatus, Core.Geocoding.Address>> Geocode(string location)
        {
            try
            {
                var response = await _http.GetStringAsync($"search?addressdetails=1&format=json&accept-language={_options.Language}&q={location}");
                Result[] results = JsonSerializer.Deserialize<Result[]>(response);

                if(results.Length == 0)
                {
                    return ErrorStatus.ZeroResults;
                }

                var address = _parser.Parse(results[0]);
                return address;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write(ex);
                return ErrorStatus.UnknownError;
            }
        }

        public Task<Either<ErrorStatus, LatLng>> Geocode(Core.Geocoding.Address address)
        {
            throw new NotImplementedException();
        }

        public Task<Either<ErrorStatus, Core.Geocoding.Address>> Geocode(decimal latitude, decimal longitude)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
