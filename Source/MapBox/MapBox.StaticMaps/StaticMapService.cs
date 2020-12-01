using System;
using System.Net.Http;
using System.Threading.Tasks;
using Proxoft.Maps.Core.StaticMaps;
using Proxoft.Maps.MapBox.Common;
using Proxoft.Maps.MapBox.StaticMaps.Helpers;

namespace Proxoft.Maps.MapBox.StaticMaps
{
    public sealed class StaticMapService : IStaticMapService, IDisposable
    {
        private readonly string _accessToken;

        private readonly HttpClient _httpClient = new ()
        {
            BaseAddress = new Uri("https://api.mapbox.com/styles/v1/mapbox/")
        };

        public StaticMapService(MapBoxOptions options)
        {
            _accessToken = $"access_token={options.AccessToken}";
        }

        public async Task<byte[]> CreateImage(MapOptions options)
        {
            try
            {
                var markerParams = options.Markers.ToQueryParameter();
                var mapParams = options.ToQueryParameter();

                var response = await _httpClient.GetAsync($"streets-v11/static/{markerParams}{mapParams}?{_accessToken}");
                if (!response.IsSuccessStatusCode)
                {
                    return Array.Empty<byte>();
                }

                var content = await response.Content.ReadAsByteArrayAsync();
                return content;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
