using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Google.Common;
using Proxoft.Maps.Google.Maps.Initialization;

namespace Proxoft.Maps.Google.Maps
{
    public class MapFactory : IMapFactory, IAsyncDisposable 
    {
        private readonly ApiLoader _api;
        private readonly GoogleApiConfiguration _configuration;

        public MapFactory(GoogleApiConfiguration configuration, IJSRuntime jsRuntime)
        {
            _api = new ApiLoader(jsRuntime);
            _configuration = configuration;
        }

        public async Task<IMap> Initialize(string cssSelector)
        {
            var status = await _api.LoadGoogleScripts(_configuration);

            Console.WriteLine($"google api load result: {status}");
            return null;
        }

        public async ValueTask DisposeAsync()
        {
            await _api.DisposeAsync();
        }
    }
}
