using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Initialization;
using Proxoft.Maps.OpenStreetMap.Maps.Models;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps
{
    public class MapFactory : IMapFactory
    {
        private readonly ApiLoader _api;
        private readonly IJSRuntime _jsRuntime;
        private readonly IIdFactory _idFactory;

        public MapFactory(
            IIdFactory idFactory,
            IJSRuntime jsRuntime)
        {
            _api = new ApiLoader(jsRuntime);
            _idFactory = idFactory;
            _jsRuntime = jsRuntime;
        }

        public string Name => "OpenStreetMaps";

        public async Task<IMap> Initialize(MapOptions options, ElementReference hostElement)
        {
            var status = await _api.LoadMapScripts();
            if (status != LoadResponse.Loaded)
            {
                return NoMap.Instance;
            }

            OsmModules modules = await OsmModules.Load(_jsRuntime);
            OsmMapObjectsFactory mapObjectsFactory = new(_idFactory, modules);
            Map map = mapObjectsFactory.CreateMap(options, hostElement);
            return map;
        }
    }
}
