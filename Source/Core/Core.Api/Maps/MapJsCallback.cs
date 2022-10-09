using System;
using System.Drawing;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Maps;

public sealed class MapJsCallback : ApiObjectJsCallback
{
    public MapJsCallback(Action<Event> onEvent) : base(onEvent)
    {
    }

    [JSInvokable]
    public void OnResized(Size newSize)
        => this.Push(new ResizedEvent(newSize));

    [JSInvokable]
    public void OnCenterChanging(LatLng latLng)
        => this.Push(new CenterChangingEvent(latLng));

    [JSInvokable]
    public void OnCenterChanged(LatLng latLng)
        => this.Push(new CenterChangedEvent(latLng));

    [JSInvokable]
    public void OnZoomChanging(int zoom)
        => this.Push(new ZoomChanging(new ZoomLevel(zoom)));

    [JSInvokable]
    public void OnZoomChanged(int zoom)
        => this.Push(new ZoomChanged(new ZoomLevel(zoom)));
}
