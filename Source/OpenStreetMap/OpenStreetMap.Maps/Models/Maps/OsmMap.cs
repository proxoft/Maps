using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps
{
    internal class OsmMap : MapBase<OsmMap>
    {
        private readonly string _mapId;

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
            this.InvokeVoidJs("AddMarker", new object[] { _mapId, options });
            return null;
        }

        public static OsmMap Create(string mapId, MapOptions options, ElementReference hostElement, IJSInProcessObjectReference jsModule)
        {
            var map = new OsmMap(mapId, jsModule);
            map.Initialize(options, hostElement);
            return map;
        }
    }
}
