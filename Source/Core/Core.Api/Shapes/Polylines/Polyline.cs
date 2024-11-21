using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public abstract class Polyline : Shape, IPolyline
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

    public void SetLatLng(LatLng[] latLngs)
    {
        this.SetLatLng(new LatLng[][] { latLngs });
    }

    public void SetLatLng(LatLng[][] latLngs)
    {
        this.InvokeVoidJs("SetLatLng", latLngs);
    }

    //public void SetStyle(Style style)
    //{
    //    this.InvokeVoidJs("SetStyle", style);
    //}

    //public void AddClass(params string[] classes)
    //{
    //    this.InvokeVoidJs("AddClass", string.Join(" ", classes));
    //}

    //public void RemoveClass(params string[] classes)
    //{
    //    this.InvokeVoidJs("RemoveClass", string.Join(" ", classes));
    //}

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
