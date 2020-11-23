using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Maps.Events;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : ApiBaseObject<T>, IMap
        where T: MapBase<T>
    {
        private readonly Subject<Event> _events = new();

        protected MapBase(IJSInProcessObjectReference jsModule) : base(jsModule)
        {
        }

        public IObservable<Event> OnEvent => _events;

        public abstract void PanTo(LatLng center);

        public abstract void ZoomTo(int zoom);

        public abstract IMarker AddMarker(MarkerOptions options);

        protected void InvokeVoidJs(string identifier, params object[] args)
            => JsModule.InvokeVoid(identifier, args);

        protected void InvokeJs<TResult>(string identifier, params object[] args)
            => JsModule.Invoke<TResult>(identifier, args);

        [JSInvokable]
        public void OnCenterChanged(LatLng latLng)
            => _events.OnNext(new CenterChangedEvent(latLng));

        [JSInvokable]
        public void OnZoomChanged(int zoom)
           => _events.OnNext(new ZoomChanged(zoom));


        protected abstract void Remove();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.Dispose();
                this.Remove();
            }

            base.Dispose(disposing);
        }
    }
}
