using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api
{
    public abstract class MarkerBase<T> : ApiBaseObject<T>, IMarker
        where T : MarkerBase<T>
    {
        private bool _isRemoved;
        private readonly MarkerJsCallback _jsCallback;

        protected MarkerBase(string markerId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _jsCallback = new MarkerJsCallback(this.Push);
            this.JsCallback = DotNetObjectReference.Create(_jsCallback);
            this.MarkerId = markerId;
        }

        protected string MarkerId { get; }

        protected DotNetObjectReference<MarkerJsCallback> JsCallback { get; private set; }

        public void SetPosition(decimal latitude, decimal longitude)
         => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

        public void AddToMap(string mapId, MarkerOptions options)
            => this.InvokeVoidJs("CreateMarker", this.MarkerId, options, mapId, this.JsCallback);

        public void SetDraggable(bool draggable)
            => this.InvokeVoidJs("SetMarkerDraggable", this.MarkerId, draggable);

        public void SetOpacity(Opacity opacity)
            => this.InvokeVoidJs("SetMarkerOpacity", this.MarkerId, opacity.Value);

        public void SetPosition(LatLng latLng)
            => this.InvokeVoidJs("SetMarkerPosition", this.MarkerId, latLng);

        public void Remove()
        {
            if (_isRemoved)
            {
                return;
            }

            _isRemoved = true;
            this.InvokeVoidJs("RemoveMarker", this.MarkerId);
        }

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
