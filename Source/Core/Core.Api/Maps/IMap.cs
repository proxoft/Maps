using Proxoft.Maps.Core.Api.Shapes.Circles;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Polylines;
using Proxoft.Maps.Core.Api.Shapes.Rectangles;

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

    IPolyline AddPolyline(PolylineOptions options);

    ICircle AddCircle(CircleOptions options);

    IRectangle AddRectangle(RectangleOptions options);

    void FitBounds(LatLngBounds bounds);

    void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom);

    LatLngBounds GetBounds();
}
