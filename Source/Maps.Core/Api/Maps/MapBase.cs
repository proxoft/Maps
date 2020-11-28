using System.Linq;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : ApiBaseObject<T>, IMap
        where T: MapBase<T>
    {
        public ApiStatus Status => ApiStatus.Available;

        protected string MapId { get; }

        protected MapBase(string mapId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            this.MapId = mapId;
        }

        public void PanTo(LatLng center)
            => this.InvokeMapJs("PanTo", center);

        public void SetCenter(LatLng position)
            => this.InvokeMapJs("SetCenter", position);

        public void ZoomTo(ZoomLevel zoom)
            => this.InvokeMapJs("ZoomTo", zoom.Value);

        public void FitBounds(LatLngBounds bounds)
            => this.FitBounds(bounds, Padding.Zero, null);

        public void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom)
            => this.InvokeMapJs("FitBounds", bounds, padding, zoom?.Value ?? null);

        public abstract IMarker AddMarker(MarkerOptions options);

        [JSInvokable]
        public void OnCenterChanged(LatLng latLng)
            => this.Push(new CenterChangedEvent(latLng));

        [JSInvokable]
        public void OnZoomChanged(int zoom)
            => this.Push(new ZoomChanged(zoom));

        [JSInvokable]
        public void OnMapClicked(LatLng latLng)
            => this.Push(new MapClickEvent(latLng));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.InvokeMapJs("Remove");
            }

            base.Dispose(disposing);
        }

        protected void InvokeMapJs(string method, params object[] args)
            => this.InvokeVoidJs(method, new object[] { this.MapId }.Concat(args).ToArray());
    }
}
