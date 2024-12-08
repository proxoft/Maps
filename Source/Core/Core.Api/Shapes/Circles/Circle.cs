using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes.Circles;

public abstract class Circle : Shape, ICircle
{
    private readonly CircleJsCallback _jsCallback;

    protected Circle(
        string id,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule) : base(id, onRemove, jsModule)
    {
        _jsCallback = new CircleJsCallback(this.Push);
    }

    public CircleType CircleType { get; private set; } = CircleType.Marker;

    internal void AddToMap(string mapId, CircleOptions options)
    {
        this.CircleType = this.IfSupportedOrDefault(options.CircleType);
        this.InvokeVoidJs("AddCircle", options, mapId, _jsCallback.DotNetRef);
    }

    public LatLng GetLatLng()
    {
        LatLng latLng = this.InvokeJs<LatLng>("GetLatLng");
        return latLng;
    }

    public void SetLatLng(LatLng latLng)
    {
        this.InvokeVoidJs("SetLatLng", latLng);
    }

    public decimal GetRadius()
    {
        decimal radius = this.InvokeJs<decimal>("GetRadius");
        return radius;
    }

    public void SetRadius(decimal radius)
    {
        this.InvokeVoidJs("SetRadius", radius);
    }

    protected sealed override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemoveCircle");
    }

    protected abstract CircleType IfSupportedOrDefault(CircleType circleType);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }
}
