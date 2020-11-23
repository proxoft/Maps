using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Maps.Events;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : IMap
        where T: MapBase<T>
    {
        private readonly Subject<Event> _events = new();

        private readonly IJSInProcessObjectReference _jsModule;

        protected MapBase(IJSInProcessObjectReference jsModule)
        {
            this.SelfRef = DotNetObjectReference.Create((T)this);
            _jsModule = jsModule;
        }

        public IObservable<Event> OnEvent => _events;

        public abstract void PanTo(LatLng center);
        public abstract void ZoomTo(int zoom);

        public abstract IMarker AddMarker(MarkerOptions options);

        protected DotNetObjectReference<T> SelfRef { get; private set; }

        protected void InvokeVoidJs(string identifier, params object[] args)
            => _jsModule.InvokeVoid(identifier, args);

        protected void InvokeJs<TResult>(string identifier, params object[] args)
            => _jsModule.Invoke<TResult>(identifier, args);

        [JSInvokable]
        public void OnCenterChanged(LatLng latLng)
            => _events.OnNext(new CenterChangedEvent(latLng));

        [JSInvokable]
        public void OnZoomChanged(int zoom)
           => _events.OnNext(new ZoomChanged(zoom));

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.Dispose();
            }
        }
    }
}
