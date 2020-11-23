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
        private readonly string _mapId;
        private readonly List<OsmMarker> _markers = new();

        public OsmMap(string mapId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _mapId = mapId;
        }

        private void Initialize(MapOptions options, ElementReference hostElement)
        {
            this.InvokeVoidJs("InitializeMapOnElement", new object[] { _mapId, options, hostElement, this.SelfRef });
        }

        public override void ZoomTo(int zoom)
            => this.InvokeVoidJs("ZoomTo", new object[] { _mapId, zoom });

        public override void PanTo(LatLng center)
            => this.InvokeVoidJs("PanTo", new object[] { _mapId, center });

        public override IMarker AddMarker(MarkerOptions options)
        {
            OsmMarker marker = new (System.Guid.NewGuid().ToString(), this.JsModule);
            marker.AddToMap(_mapId, options);
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
                foreach(var m in _markers)
                {
                    m.Dispose();
                }

                _markers.Clear();
            }

            base.Dispose(disposing);
        }

        protected override void Remove()
        {
            this.InvokeVoidJs("Remove", _mapId);
        }
    }
}
