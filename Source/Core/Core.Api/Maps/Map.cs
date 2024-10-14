using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Shapes;
using Proxoft.Maps.Core.Api.Shapes.Polylines;

namespace Proxoft.Maps.Core.Api.Maps;

public abstract class Map : ApiObject, IMap
{
    private readonly MapJsCallback _mapJsCallback;
    private readonly IMapObjectsFactory _mapObjectsFactory;

    private readonly List<Marker> _markers = new();
    private readonly List<Polygon> _polygons = new();
    private readonly List<Polyline> _polylines = new();

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

    public void Initialize(MapOptions options, ElementReference hostElement)
    {
        this.InvokeVoidJs("InitializeMapOnElement", new object[] { options, hostElement, _mapJsCallback.DotNetRef });
    }

    protected override sealed void ExecuteRemove()
    {
        this.InvokeVoidJs("Remove");

        _markers.DisposeAll();
        _polygons.DisposeAll();
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
}
