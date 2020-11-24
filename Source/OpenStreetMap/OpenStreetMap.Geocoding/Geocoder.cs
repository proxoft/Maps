using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Geocoding;
using Proxoft.Maps.OpenStreetMap.Common;

namespace OpenStreetMap.Geocoding
{
    public sealed class Geocoder : IGeocoder, IDisposable
    {
        private readonly OpenStreetMapOptions _options;
        private readonly HttpClient _http = new()
        {
            BaseAddress= new Uri("https://nominatim.openstreetmap.org/")
        };

        public Geocoder(OpenStreetMapOptions options)
        {
            _options = options;
        }

        public async Task<Either<ErrorStatus, Address>> Geocode(string location)
        {
            var response = await _http.GetStringAsync($"search?addressdetails=1&limit=1&q={location}");
            Models.Result result = JsonSerializer.Deserialize<Models.Result>(response);

            return new Address();
        }

        public Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
