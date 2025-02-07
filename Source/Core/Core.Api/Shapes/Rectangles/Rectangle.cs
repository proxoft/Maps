using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

public abstract class Rectangle : Shape, IRectangle
{
    private readonly RectangleJsCallback _jsCallback;

    protected Rectangle(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : base(id, onRemove, jsModule)
    {
        _jsCallback = new(this.Push);
    }

    public void AddToMap(string mapId, RectangleOptions options)
    {
        this.InvokeVoidJs("AddRectangle", options, mapId, _jsCallback.DotNetRef);
    }

    public LatLngBounds GetBounds()
    {
        LatLng[] corners = this.InvokeJs<LatLng[]>("GetBounds");
        return LatLngBounds.FromCorners(corners[0], corners[1]);
    }

    public LatLng GetCenter()
    {
        LatLngBounds bounds = this.GetBounds();
        return bounds.Center;
    }

    public void SetCenter(LatLng latLng)
    {
        LatLngBounds bounds = this.GetBounds();
        bounds = bounds.MoveCenter(latLng);
        this.SetBounds(bounds);
    }

    public void SetBounds(LatLngBounds bounds)
    {
        this.InvokeVoidJs("SetBounds", bounds);
    }

    protected override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemoveRectangle");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }

    public void SetDraggable(bool draggable)
    {
        this.InvokeVoidJs("SetRectangleDraggable", draggable);
    }
}
