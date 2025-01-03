﻿using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public abstract class Polygon : Shape, IPolygon
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

    public void SetLatLngs(PolygonLatLng latLngs)
    {
        this.InvokeVoidJs("SetLatLngs", latLngs);
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
        if (disposing)
        {
            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }
}
