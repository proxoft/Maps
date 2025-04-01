using System;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api.Shapes.Circles;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Polylines;
using Proxoft.Maps.Core.Api.Shapes.Rectangles;

namespace Proxoft.Maps.Core.Api;

public sealed class NoMap : IMap
{
    public static readonly NoMap Instance = new();

    private NoMap()
    {
    }

    public IObservable<Event> OnEvent => Observable.Never<Event>();

    public ApiStatus Status => ApiStatus.NotAvailable;

    public string Id => "none-map";

    public bool IsRemoved => false;

    public IMarker AddMarker(MarkerOptions options)
        => NoMarker.Instance;

    public IPolygon AddPolygon(PolygonOptions options)
        => NoPolygon.Instance;

    public IPolyline AddPolyline(PolylineOptions options)
        => NoPolyline.Instance;

    public ICircle AddCircle(CircleOptions options)
        => NoCircle.Instance;

    public IRectangle AddRectangle(RectangleOptions options)
       => NoRectangle.Instance;

    public void FitBounds(LatLngBounds bounds)
    {
    }

    public void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom)
    {
    }

    public LatLngBounds GetBounds()
    {
        return LatLngBounds.Empty;
    }

    public void PanTo(LatLng center)
    {
    }

    public void SetCenter(LatLng position)
    {
    }

    public LatLng GetCenter()
    {
        return LatLng.None;
    }

    public void ZoomTo(ZoomLevel zoom)
    {
    }

    public void Remove()
    {
    }

    public void Dispose()
    {
    }

    public void SetDraggable(bool draggable)
    {
    }

    public bool IsDraggable()
    {
        return false;
    }

    public void InvalidateSize()
    {
    }
}
