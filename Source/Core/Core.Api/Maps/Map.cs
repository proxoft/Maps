using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes;

namespace Proxoft.Maps.Core.Api.Maps;

public abstract class Map : ApiObject, IMap
{
    private readonly MapJsCallback _mapJsCallback;

    protected Map(
        string mapId,
        IJSInProcessObjectReference jsModule) : base(mapId, _ => { }, jsModule)
    {
        this.MapId = mapId;

        _mapJsCallback = new MapJsCallback(this.Push);
    }

    public ApiStatus Status => ApiStatus.Available;

    protected string MapId { get; }

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

    public abstract IMarker AddMarker(MarkerOptions options);

    public abstract IPolygon AddPolygon(PolygonOptions options);

    protected override void ExecuteRemove()
    {
        this.InvokeVoidJs("Remove");
    }

    protected void Initialize(MapOptions options, ElementReference hostElement)
    {
        this.InvokeVoidJs("InitializeMapOnElement", new object[] { options, hostElement, _mapJsCallback.DotNetRef });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _mapJsCallback.Dispose();
            this.Remove();
        }

        base.Dispose(disposing);
    }

    //protected void InvokeMapJs(string method, params object?[] args)
    //    => this.InvokeVoidJs(method, new object?[] { this.MapId }.Concat(args).ToArray());

    //protected TResult InvokeMapJs<TResult>(string method, params object?[] args)
    //    => this.InvokeJs<TResult>(method, new object?[] { this.MapId }.Concat(args).ToArray());
}
