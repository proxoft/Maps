using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api
{
    public interface IMap : IApiObject
    {
        ApiStatus Status { get; }

        void SetCenter(LatLng position);

        void PanTo(LatLng position);

        void ZoomTo(ZoomLevel zoom);

        IMarker AddMarker(MarkerOptions options);

        void FitBounds(LatLngBounds bounds);
        void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom);
    }
}
