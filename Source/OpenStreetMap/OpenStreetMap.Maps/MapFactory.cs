using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Initialization;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps
{
    public class MapFactory : IMapFactory
    {
        private readonly Lazy<Task<IJSInProcessObjectReference>> _moduleTask;
        private readonly ApiLoader _api;

        public MapFactory(IJSRuntime jsRuntime)
        {
            _api = new ApiLoader(jsRuntime);

            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
               "import",
               "./_content/Proxoft.Maps.OpenStreetMap.Maps/maps.js").AsTask());
        }

        public string Name => "OpenStreetMaps";

        public async Task<IMap> Initialize(string elementId, MapOptions options)
        {
            var status = await _api.LoadGoogleScripts();
            if (status != ApiStatus.Loaded)
            {
                return NoMap.Instance;
            }

            var module = await _moduleTask.Value;
            var map = OsmMap.Create(elementId, options, module);
            return map;
        }
    }
}
