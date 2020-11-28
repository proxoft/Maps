using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : ApiBaseObject<T>, IMap
        where T: MapBase<T>
    {
        public ApiStatus Status => ApiStatus.Available;

        protected MapBase(IJSInProcessObjectReference jsModule) : base(jsModule)
        {
        }

        public abstract void PanTo(LatLng center);

        public abstract void ZoomTo(int zoom);

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

        protected abstract void Remove();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Remove();
            }

            base.Dispose(disposing);
        }
    }
}
