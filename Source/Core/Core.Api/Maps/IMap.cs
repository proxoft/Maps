using Proxoft.Maps.Core.Api.Core;
using Proxoft.Maps.Core.Api.Shapes;

namespace Proxoft.Maps.Core.Api;

public interface IMap : IApiObject
{
    ApiStatus Status { get; }

    void SetCenter(LatLng position);

    LatLng GetCenter();

    void PanTo(LatLng position);

    void ZoomTo(ZoomLevel zoom);

    IMarker AddMarker(MarkerOptions options);

    IPolygon AddPolygon(PolygonOptions options);

    void FitBounds(LatLngBounds bounds);

    void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom);

    LatLngBounds GetBounds();
}
