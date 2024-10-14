using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public class Polyline : ApiObject, IPolyline
{
    private readonly PolylineJsCallback _jsCallback;

    public Polyline(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : base(id, onRemove, jsModule)
    {
        _jsCallback = new PolylineJsCallback(this.Push);
    }

    public void AddToMap(string mapId, PolylineOptions options)
    {
        this.InvokeVoidJs("AddPolyline", options, mapId, _jsCallback.DotNetRef);
    }

    public LatLngBounds GetBounds()
    {
        LatLng[] corners = this.InvokeJs<LatLng[]>("GetBounds");
        return LatLngBounds.FromCorners(corners[0], corners[1]);
    }

    public PolylineLatLng GetLatLngs()
    {
        PolylineLatLng latLngs = this.InvokeJs<PolylineLatLng>("GetLatLngs");
        return latLngs;
    }

    public void SetLatLngs(LatLng[] latLngs)
    {
        this.SetLatLngs(new LatLng[][] { latLngs });
    }

    public void SetLatLngs(LatLng[][] latLngs)
    {
        this.InvokeVoidJs("SetLatLngs", latLngs);
    }

    public void SetStyle(Style style)
    {
        this.InvokeVoidJs("SetStyle", style);
    }

    protected override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemovePolyline");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }
}
