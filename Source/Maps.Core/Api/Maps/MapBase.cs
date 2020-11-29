using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Maps
{
    public abstract class MapBase<T> : ApiBaseObject<T>, IMap
        where T: MapBase<T>
    {
        private readonly MapJsCallback _mapJsCallback;

        protected MapBase(string mapId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            this.MapId = mapId;
            _mapJsCallback = new MapJsCallback(this.Push);
            this.MapJsCallback = DotNetObjectReference.Create(_mapJsCallback);
        }

        public ApiStatus Status => ApiStatus.Available;

        protected string MapId { get; }

        protected DotNetObjectReference<MapJsCallback> MapJsCallback { get; private set; }

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

        protected void Initialize(MapOptions options, ElementReference hostElement)
        {
            this.InvokeVoidJs("InitializeMapOnElement", new object[] { this.MapId, options, hostElement, this.MapJsCallback });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mapJsCallback.Dispose();
                this.InvokeMapJs("Remove");
            }

            base.Dispose(disposing);
        }

        protected void InvokeMapJs(string method, params object[] args)
            => this.InvokeVoidJs(method, new object[] { this.MapId }.Concat(args).ToArray());
    }
}
