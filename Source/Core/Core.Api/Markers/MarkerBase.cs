using Microsoft.JSInterop;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api.Icons;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api;

public abstract class MarkerBase : ApiObject, IMarker
{
    private readonly MarkerJsCallback _jsCallback;

    protected MarkerBase(string markerId, IJSInProcessObjectReference jsModule) : base(markerId, jsModule)
    {
        _jsCallback = new MarkerJsCallback(this.Push);
    }

    public void SetPosition(decimal latitude, decimal longitude)
     => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

    public void AddToMap(string mapId, MarkerOptions options)
    {
        this.InvokeVoidJs("AddMarker", this.Id, options, options.Icon, mapId, _jsCallback.DotNetRef);
    }

    public void SetDraggable(bool draggable)
        => this.InvokeVoidJs("SetMarkerDraggable", this.Id, draggable);

    public void SetOpacity(Opacity opacity)
        => this.InvokeVoidJs("SetMarkerOpacity", this.Id, (decimal)opacity);

    public void SetPosition(LatLng latLng)
        => this.InvokeVoidJs("SetMarkerPosition", this.Id, latLng);

    public void SetIcon(IconOptions icon)
    {
        this.InvokeVoidJs("SetMarkerIcon", this.Id, icon);
    }

    protected override void ExecuteRemove()
    {
        this.InvokeVoidJs("RemoveMarker", this.Id);
    }

    protected override void InvokeVoidJs(string identifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new System.Exception("Marker has been removed from the map. Do not use it anymore. If necessary create new marker");
        }

        base.InvokeVoidJs(identifier, args);
    }

    protected override TResult InvokeJs<TResult>(string identifier, params object?[] args)
    {
        if (this.IsRemoved)
        {
            throw new System.Exception("Marker has been removed from the map. Do not use it anymore. If necessary create new marker");
        }

        return base.InvokeJs<TResult>(identifier, args);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Remove();

            _jsCallback.Dispose();
        }

        base.Dispose(disposing);
    }
}
