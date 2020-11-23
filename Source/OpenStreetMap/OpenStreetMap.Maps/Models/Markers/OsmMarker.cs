using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class OsmMarker : MarkerBase<OsmMarker>, IMarker
    {
        private readonly string _markerId;

        public OsmMarker(string markerId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _markerId = markerId;
        }

        public void AddToMap(string mapId, MarkerOptions options)
            => this.InvokeVoidJs("CreateMarker", _markerId, options, mapId, this.SelfRef);

        public override void SetDraggable(bool draggable)
            => this.InvokeVoidJs("SetMarkerDraggable", _markerId, draggable);

        public override void SetPosition(LatLng latLng)
            => this.InvokeVoidJs("SetMarkerPosition", _markerId, latLng);
    }
}
