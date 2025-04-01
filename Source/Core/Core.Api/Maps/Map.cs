using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Shapes.Circles;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Polylines;
using Proxoft.Maps.Core.Api.Shapes.Rectangles;

namespace Proxoft.Maps.Core.Api.Maps;

public abstract class Map : ApiObject, IMap
{
    private readonly MapJsCallback _mapJsCallback;
    private readonly IMapObjectsFactory _mapObjectsFactory;

    private readonly List<Marker> _markers = [];
    private readonly List<Polygon> _polygons = [];
    private readonly List<Polyline> _polylines = [];
    private readonly List<Circle> _circles = [];
    private readonly List<Rectangle> _rectangles = [];

    protected Map(
        string mapId,
        IMapObjectsFactory mapObjectsFactory,
        IJSInProcessObjectReference jsModule) : base(mapId, _ => { }, jsModule)
    {
        _mapObjectsFactory = mapObjectsFactory;
        _mapJsCallback = new MapJsCallback(this.Push);
    }

    public ApiStatus Status => ApiStatus.Available;

    public void PanTo(LatLng center)
        => this.InvokeVoidJs("PanTo", center);

    public void SetCenter(LatLng position)
        => this.InvokeVoidJs("SetCenter", position);

    public LatLng GetCenter()
    {
        LatLng center = this.InvokeJs<LatLng>("GetCenter");
        return center;
    }

    public void ZoomTo(ZoomLevel zoom)
        => this.InvokeVoidJs("ZoomTo", (decimal)zoom);

    public void FitBounds(LatLngBounds bounds)
        => this.FitBounds(bounds, Padding.Zero, ZoomLevel.Default);

    public void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom)
        => this.InvokeVoidJs("FitBounds", bounds, padding, zoom == ZoomLevel.Default ? null : (decimal)zoom);

    public LatLngBounds GetBounds()
    {
        LatLng[] corners = this.InvokeJs<LatLng[]>("GetBounds");
        return LatLngBounds.FromCorners(corners[0], corners[1]);
    }

    public void SetDraggable(bool draggable)
    {
        this.InvokeVoidJs("SetDraggable", draggable);
    }

    public bool IsDraggable()
    {
        bool isDraggable = this.InvokeJs<bool>("IsDraggable");
        return isDraggable;
    }

    public IMarker AddMarker(MarkerOptions options)
    {
        Marker marker = _mapObjectsFactory.CreateMarker(this.OnMarkerRemove);
        _markers.Add(marker);
        marker.AddToMap(this.Id, options);

        return marker;
    }

    public IPolygon AddPolygon(PolygonOptions options)
    {
        Polygon polygon = _mapObjectsFactory.CreatePolygon(this.OnPolygonRemove);
        _polygons.Add(polygon);
        polygon.AddToMap(this.Id, options);
        return polygon;
    }

    public IPolyline AddPolyline(PolylineOptions options)
    {
        Polyline polyline = _mapObjectsFactory.CreatePolyline(this.OnPolylineRemove);
        _polylines.Add(polyline);
        polyline.AddToMap(this.Id, options);
        return polyline;
    }

    public ICircle AddCircle(CircleOptions options)
    {
        Circle circle = _mapObjectsFactory.CreateCircle(this.OnCircleRemove);
        _circles.Add(circle);
        circle.AddToMap(this.Id, options);
        return circle;
    }

    public IRectangle AddRectangle(RectangleOptions options)
    {
        Rectangle rectangle = _mapObjectsFactory.CreateRectangle(this.OnRectangleRemove);
        _rectangles.Add(rectangle);
        rectangle.AddToMap(this.Id, options);
        return rectangle;
    }

    public void Initialize(MapOptions options, ElementReference hostElement)
    {
        this.InvokeVoidJs("InitializeMapOnElement", [options, hostElement, _mapJsCallback.DotNetRef]);
    }

    public void InvalidateSize()
    {
        this.InvokeVoidJs("InvalidateSize");
    }

    protected override sealed void ExecuteRemove()
    {
        this.InvokeVoidJs("Remove");

        _markers.DisposeAll();
        _polygons.DisposeAll();
        _polylines.DisposeAll();
        _circles.DisposeAll();
        _rectangles.DisposeAll();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _mapJsCallback.Dispose();
        }

        base.Dispose(disposing);
    }

    private void OnMarkerRemove(string markerId)
    {
        var i = _markers.FindIndex(m => m.Id == markerId);
        _markers.RemoveAt(i);
    }

    private void OnPolygonRemove(string polygonId)
    {
        var i = _polygons.FindIndex(p => p.Id == polygonId);
        _polygons.RemoveAt(i);
    }

    private void OnPolylineRemove(string polylineId)
    {
        var i = _polylines.FindIndex(p => p.Id == polylineId);
        _polylines.RemoveAt(i);
    }

    private void OnCircleRemove(string circleId)
    {
        int i = _circles.FindIndex(c => c.Id == circleId);
        _circles.RemoveAt(i);
    }

    private void OnRectangleRemove(string rectangleId)
    {
        int i = _rectangles.FindIndex(c => c.Id == rectangleId);
        _rectangles.RemoveAt(i);
    }
}
