using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Api.Shapes;

public abstract class Polygon : ApiObject, IPolygon
{
    private readonly PolygonJsCallback _jsCallback;

    protected Polygon(
        string id,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule) : base(id, onRemove, jsModule)
    {
        _jsCallback = new PolygonJsCallback(this.Push);
    }

    public LatLngBounds GetBounds()
    {
        LatLng[] corners = this.InvokeJs<LatLng[]>("GetBounds");
        return LatLngBounds.FromCorners(corners[0], corners[1]);
    }

    public PolygonLatLng GetLatLngs()
    {
        PolygonLatLng latLngs = this.InvokeJs<PolygonLatLng>("GetLatLngs");
        return latLngs;
    }

    public void SetLatLng(PolygonLatLng latLngs)
    {
        this.InvokeVoidJs("SetLatLng", latLngs);
    }

    public void SetStyle(Style style)
    {
        this.InvokeVoidJs("SetStyle", style);
    }

    public void AddToMap(string mapId, PolygonOptions options)
    {
        this.InvokeVoidJs("AddPolygon", options, mapId, _jsCallback.DotNetRef);
    }

    protected sealed override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemovePolygon");
    }

    protected override void Dispose(bool disposing)
    {
        if(disposing)
        {
            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }
}
