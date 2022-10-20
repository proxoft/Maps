using Microsoft.JSInterop;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api.Icons;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api;

public abstract class MarkerBase : ApiBaseObject, IMarker
{
    private readonly MarkerJsCallback _jsCallback;

    protected MarkerBase(string markerId, IJSInProcessObjectReference jsModule) : base(jsModule)
    {
        _jsCallback = new MarkerJsCallback(this.Push);

        this.MarkerId = markerId;
    }

    public string MarkerId { get; }

    public bool IsRemoved { get; private set; }

    public void SetPosition(decimal latitude, decimal longitude)
     => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

    public void AddToMap(string mapId, MarkerOptions options)
    {
        this.InvokeVoidJs("AddMarker", this.MarkerId, options, options.Icon, mapId, _jsCallback.DotNetRef);
    }

    public void SetDraggable(bool draggable)
        => this.InvokeVoidJs("SetMarkerDraggable", this.MarkerId, draggable);

    public void SetOpacity(Opacity opacity)
        => this.InvokeVoidJs("SetMarkerOpacity", this.MarkerId, (decimal)opacity);

    public void SetPosition(LatLng latLng)
        => this.InvokeVoidJs("SetMarkerPosition", this.MarkerId, latLng);

    public void SetIcon(IconOptions icon)
    {
        this.InvokeVoidJs("SetMarkerIcon", this.MarkerId, icon);
    }

    public virtual void Remove()
    {
        if (IsRemoved)
        {
            return;
        }

        this.InvokeVoidJs("RemoveMarker", this.MarkerId);
        IsRemoved = true;
    }

    protected override void InvokeVoidJs(string identifier, params object[] args)
    {
        if (this.IsRemoved)
        {
            throw new System.Exception("Marker has been removed from the map. Do not use it anymore. If necessary create new marker");
        }

        base.InvokeVoidJs(identifier, args);
    }

    protected override TResult InvokeJs<TResult>(string identifier, params object[] args)
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
