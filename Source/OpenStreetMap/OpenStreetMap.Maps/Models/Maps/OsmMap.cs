using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Markers;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps
{
    internal class OsmMap : MapBase<OsmMap>
    {
        private readonly List<OsmMarker> _markers = new();
        private readonly Hooks _markerHooks;

        private OsmMap(string mapId, IJSInProcessObjectReference jsModule) : base(mapId, jsModule)
        {
            _markerHooks = new()
            {
                OnRemove = this.OnMarkerRemove
            };
        }

        public override IMarker AddMarker(MarkerOptions options)
        {
            OsmMarker marker = new (System.Guid.NewGuid().ToString(), this.JsModule, _markerHooks);

            _markers.Add(marker);
            marker.AddToMap(this.MapId, options);
            return marker;
        }

        public static OsmMap Create(string mapId, MapOptions options, ElementReference hostElement, IJSInProcessObjectReference jsModule)
        {
            var map = new OsmMap(mapId, jsModule);
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
