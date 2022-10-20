using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Markers;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps
{
    internal class OsmMap : MapBase
    {
        private readonly List<OsmMarker> _markers = new();
        private readonly Hooks _markerHooks;
        private readonly OsmModules _modules;

        private OsmMap(
            string mapId,
            OsmModules modules) : base(mapId, modules.Map)
        {
            _markerHooks = new()
            {
                OnRemove = this.OnMarkerRemove
            };
            _modules = modules;
        }

        public override IMarker AddMarker(MarkerOptions options)
        {
            OsmMarker marker = new (System.Guid.NewGuid().ToString(), _modules.Marker, _markerHooks);

            _markers.Add(marker);
            marker.AddToMap(this.MapId, options);
            return marker;
        }

        public static OsmMap Create(
            string mapId,
            MapOptions options,
            ElementReference hostElement,
            OsmModules modules)
        {
            var map = new OsmMap(mapId, modules);
            map.Initialize(options, hostElement);
            return map;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _markerHooks.OnRemove = null;

                foreach(var m in _markers)
                {
                    m.Dispose();
                }

                _markers.Clear();
            }

            base.Dispose(disposing);
        }

        private void OnMarkerRemove(string markerId)
        {
            var i = _markers.FindIndex(m => m.MarkerId == markerId);
            _markers.RemoveAt(i);
        }
    }
}
