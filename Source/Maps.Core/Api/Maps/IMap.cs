using System;

namespace Proxoft.Maps.Core.Api
{
    public interface IMap : IDisposable
    {
        IObservable<LatLng> OnCenter { get; }
        IObservable<int> OnZoom { get; }

        void PanTo(LatLng center);

        void ZoomTo(int zoom);

        IMarker AddMarker(MarkerOptions options);
    }
}
