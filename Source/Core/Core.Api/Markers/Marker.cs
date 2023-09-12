using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api.Icons;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api;

public abstract class Marker : ApiObject, IMarker
{
    private readonly MarkerJsCallback _jsCallback;

    protected Marker(
        string markerId,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule) : base(markerId, onRemove, jsModule)
    {
        _jsCallback = new MarkerJsCallback(this.Push);
    }

    public void SetPosition(decimal latitude, decimal longitude)
        => this.SetPosition(new LatLng { Latitude = latitude, Longitude = longitude });

    public void AddToMap(string mapId, MarkerOptions options)
    {
        this.InvokeVoidJs("AddMarker", options, options.Icon, mapId, _jsCallback.DotNetRef);
    }

    public void SetDraggable(bool draggable)
        => this.InvokeVoidJs("SetMarkerDraggable", draggable);

    public void SetOpacity(Opacity opacity)
        => this.InvokeVoidJs("SetMarkerOpacity", (decimal)opacity);

    public void SetPosition(LatLng latLng)
        => this.InvokeVoidJs("SetMarkerPosition", latLng);

    public void SetIcon(IconOptions icon)
    {
        this.InvokeVoidJs("SetMarkerIcon", icon);
    }

    protected override sealed void ExecuteRemove()
    {
        this.InvokeVoidJs("RemoveMarker");
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
