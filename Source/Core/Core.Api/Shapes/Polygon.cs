using System;
using Microsoft.JSInterop;

namespace Proxoft.Maps.Core.Api.Shapes;

public abstract class Polygon : ApiObject, IPolygon
{
    private readonly PolygonJsCallback _jsCallback;
    private readonly Action<string> _onRemoved;

    protected Polygon(
        string id,
        IJSInProcessObjectReference jsModule,
        Action<string> onRemoved) : base(id, jsModule)
    {
        _jsCallback = new PolygonJsCallback(this.Push);
        _onRemoved = onRemoved;
    }

    public bool IsRemoved { get; private set; }

    public LatLngBounds GetBounds()
    {
        return new LatLngBounds();
    }

    public PolygonLatLng GetLatLngs()
    {
        return new PolygonLatLng();
    }

    public void SetLatLng(PolygonLatLng latLngs)
    {
    }

    public void AddToMap(string mapId, PolygonOptions options)
    {
        this.InvokeVoidJs("AddPolygon", this.Id, options, mapId, _jsCallback.DotNetRef);
    }

    public void Remove()
    {
        if (IsRemoved)
        {
            return;
        }

        this.InvokeVoidJs("RemovePolygon", this.Id);
        IsRemoved = true;
        _onRemoved(this.Id);
    }
}
