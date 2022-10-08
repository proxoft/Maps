using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api
{
    public abstract class MarkerBase<T> : ApiBaseObject<T>, IMarker
        where T : MarkerBase<T>
    {
        private readonly MarkerJsCallback _jsCallback;

        protected MarkerBase(string markerId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _jsCallback = new MarkerJsCallback(this.Push);
            this.JsCallback = DotNetObjectReference.Create(_jsCallback);
            this.MarkerId = markerId;
        }

        public string MarkerId { get; }
        public bool IsRemoved { get; private set; }

        protected DotNetObjectReference<MarkerJsCallback> JsCallback { get; private set; }

        public void SetPosition(decimal latitude, decimal longitude)
         => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

        public void AddToMap(string mapId, MarkerOptions options)
            => this.InvokeVoidJs("AddMarker", this.MarkerId, options, mapId, this.JsCallback);

        public void SetDraggable(bool draggable)
            => this.InvokeVoidJs("SetMarkerDraggable", this.MarkerId, draggable);

        public void SetOpacity(Opacity opacity)
            => this.InvokeVoidJs("SetMarkerOpacity", this.MarkerId, (decimal)opacity);

        public void SetPosition(LatLng latLng)
            => this.InvokeVoidJs("SetMarkerPosition", this.MarkerId, latLng);

        public virtual void Remove()
        {
            if (IsRemoved)
            {
                return;
            }

            this.InvokeVoidJs("RemoveMarker", this.MarkerId);
            IsRemoved = true;
        }

        protected override void InvokeVoidJs(string identifier, params object[] args)
        {
            if (this.IsRemoved)
            {
                throw new System.Exception("Marker has been removed from the map. Do not use it anymore. If necessary create new marker");
            }

            base.InvokeVoidJs(identifier, args);
        }

        protected override TResult InvokeJs<TResult>(string identifier, params object[] args)
        {
            if (this.IsRemoved)
            {
                throw new System.Exception("Marker has been removed from the map. Do not use it anymore. If necessary create new marker");
            }

            return base.InvokeJs<TResult>(identifier, args);
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
