using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : IMap
        where T: MapBase<T>
    {
        private readonly Subject<LatLng> _onCenter = new();
        private readonly Subject<int> _onZoom = new();

        private readonly IJSInProcessObjectReference _jsModule;

        protected MapBase(IJSInProcessObjectReference jsModule)
        {
            this.SelfRef = DotNetObjectReference.Create((T)this);
            _jsModule = jsModule;
        }

        public IObservable<LatLng> OnCenter => _onCenter;
        public IObservable<int> OnZoom => _onZoom;

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
            => _onCenter.OnNext(latLng);

        [JSInvokable]
        public void OnZoomChanged(int zoom)
           => _onZoom.OnNext(zoom);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onCenter.Dispose();
            }
        }
    }
}
