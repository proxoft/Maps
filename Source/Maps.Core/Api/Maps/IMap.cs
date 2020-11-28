using System;

namespace Proxoft.Maps.Core.Api
{
    public interface IMap : IDisposable
    {
        ApiStatus Status { get; }

        IObservable<Event> OnEvent { get; }

        void SetCenter(LatLng position);

        void PanTo(LatLng position);

        void ZoomTo(ZoomLevel zoom);

        IMarker AddMarker(MarkerOptions options);

        void FitBounds(LatLngBounds bounds);
        void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom);
    }
}
