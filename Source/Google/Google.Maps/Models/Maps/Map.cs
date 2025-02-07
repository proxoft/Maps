using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes.Circles;
using Proxoft.Maps.Core.Api.Shapes.Polygones;
using Proxoft.Maps.Core.Api.Shapes.Polylines;
using Proxoft.Maps.Core.Api.Shapes.Rectangles;

namespace Proxoft.Maps.Google.Maps.Models.Maps;

internal class GoogleMap : IMap
{
    private readonly string _elementId;
    private readonly IJSInProcessObjectReference _jsRuntime;
    private readonly DotNetObjectReference<GoogleMap> _ref;

    private readonly Subject<LatLng> _onCenter = new ();

    public IObservable<LatLng> OnCenter => _onCenter;

    public IObservable<int> OnZoom => throw new NotImplementedException();

    public IObservable<Event> OnEvent => throw new NotImplementedException();

    public ApiStatus Status => ApiStatus.Available;

    public string Id => "g-map";

    public bool IsRemoved => throw new NotImplementedException();

    [JSInvokable]
    public void OnCenterChanged(LatLng latLng)
        => _onCenter.OnNext(latLng);

    private GoogleMap(string elementId, IJSInProcessObjectReference jsRuntime)
    {
        _elementId = elementId;
        _jsRuntime = jsRuntime;
        _ref = DotNetObjectReference.Create(this);
    }

    private void Initialize(MapOptions options)
    {
        _jsRuntime.InvokeVoid("InitializeMap", new object[] { _elementId, options, _ref });
    }

    public static GoogleMap Create(string elementId, MapOptions options, IJSInProcessObjectReference jsRuntime)
    {
        var map = new GoogleMap(elementId, jsRuntime);
        map.Initialize(options);
        return map;
    }

    public void Dispose()
    {
        _onCenter.Dispose();
    }

    public void PanTo(LatLng center)
    {
        throw new NotImplementedException();
    }

    public IMarker AddMarker(MarkerOptions options)
    {
        throw new NotImplementedException();
    }

    public void ZoomTo(int zoom)
    {
        throw new NotImplementedException();
    }

    public void SetCenter(LatLng position)
    {
        throw new NotImplementedException();
    }

    public void ZoomTo(ZoomLevel zoom)
    {
        throw new NotImplementedException();
    }

    public void FitBounds(LatLngBounds bounds)
    {
        throw new NotImplementedException();
    }

    public void FitBounds(LatLngBounds bounds, Padding padding, ZoomLevel zoom)
    {
        throw new NotImplementedException();
    }

    public LatLngBounds GetBounds()
    {
        throw new NotImplementedException();
    }

    public LatLng GetCenter()
    {
        throw new NotImplementedException();
    }

    public IPolygon AddPolygon(PolygonOptions options)
    {
        throw new NotImplementedException();
    }

    public void Remove()
    {
        throw new NotImplementedException();
    }

    public IPolyline AddPolyline(PolylineOptions options)
    {
        throw new NotImplementedException();
    }

    public ICircle AddCircle(CircleOptions options)
    {
        throw new NotImplementedException();
    }

    public IRectangle AddRectangle(RectangleOptions options)
    {
        throw new NotImplementedException();
    }

    public void SetDraggable(bool draggable)
    {
        throw new NotImplementedException();
    }

    public bool IsDraggable()
    {
        throw new NotImplementedException();
    }
}
