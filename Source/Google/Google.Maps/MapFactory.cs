using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.Google.Common;
using Proxoft.Maps.Google.Maps.Initialization;
using Proxoft.Maps.Google.Maps.Models.Maps;

namespace Proxoft.Maps.Google.Maps
{
    public class MapFactory : IMapFactory, IAsyncDisposable 
    {
        private readonly Lazy<Task<IJSInProcessObjectReference>> _moduleTask;
        private readonly ApiLoader _api;
        private readonly GoogleApiConfiguration _configuration;

        public string Name => "GoogleMaps";

        public MapFactory(GoogleApiConfiguration configuration, IJSRuntime jsRuntime)
        {
            _api = new ApiLoader(jsRuntime);
            _configuration = configuration;

            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
               "import",
               "./_content/Proxoft.Maps.Google.Maps/maps.js").AsTask());
        }

        public async Task<IMap> Initialize(string elementId, MapOptions options)
        {
            var status = await _api.LoadGoogleScripts(_configuration);
            if (status != LoadResponse.Loaded)
            {
                return NoMap.Instance;
            }

            var m = await _moduleTask.Value;
            var map = GoogleMap.Create(elementId, options, m);
            return map;
        }

        public async ValueTask DisposeAsync()
        {
            await _api.DisposeAsync();
        }

        public Task<IMap> Initialize(MapOptions options, ElementReference hostElement)
        {
            throw new NotImplementedException();
        }
    }
}
