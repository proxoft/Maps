using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
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
               "./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_1.0.1.js").AsTask());
        }

        public string Name => "OpenStreetMaps";

        public async Task<IMap> Initialize(MapOptions options, ElementReference hostElement)
        {
            var status = await _api.LoadGoogleScripts();
            if (status != LoadResponse.Loaded)
            {
                return NoMap.Instance;
            }

            var mapId = Guid.NewGuid().ToString();
            var module = await _moduleTask.Value;
            var map = OsmMap.Create(mapId, options, hostElement, module);
            return map;
        }
    }
}
