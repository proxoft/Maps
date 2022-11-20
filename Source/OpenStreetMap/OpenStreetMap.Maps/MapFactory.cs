﻿using System;
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
        private readonly ApiLoader _api;
        private readonly IJSRuntime _jsRuntime;

        public MapFactory(IJSRuntime jsRuntime)
        {
            _api = new ApiLoader(jsRuntime);
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

            var mapId = Guid.NewGuid().ToString();

            OsmModules modules = await OsmModules.Load(_jsRuntime);
            var map = OsmMap.Create(mapId, options, hostElement, modules);
            return map;
        }
    }
}
