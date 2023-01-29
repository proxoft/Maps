using System;
using Microsoft.JSInterop;

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

    protected sealed override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemovePolygon", this.Id);
    }
}
