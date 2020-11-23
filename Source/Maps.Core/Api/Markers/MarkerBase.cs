using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api
{
    public abstract class MarkerBase<T> : ApiBaseObject<T>, IMarker
        where T : MarkerBase<T>
    {
        protected MarkerBase(IJSInProcessObjectReference jsModule) : base(jsModule)
        {
        }

        public abstract void SetDraggable(bool draggable);

        public void SetPosition(decimal latitude, decimal longitude)
         => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

        public abstract void SetPosition(LatLng latLng);

        public abstract void SetOpacity(Opacity opacity);

        [JSInvokable]
        public void OnPositionChanged(LatLng position)
            => this.Push(new MarkerPositionChangedEvent(position));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}
