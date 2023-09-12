using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Markers;

internal class MarkerJsCallback : ApiObjectJsCallback<MarkerJsCallback>
{
    public MarkerJsCallback(Action<Event> onEvent) : base(onEvent)
    {
    }

    [JSInvokable]
    public void OnPositionChanged(LatLng position)
        => this.Push(new PositionChangedEvent(position));

    [JSInvokable]
    public void OnDrag(LatLng position)
        => this.Push(new DragEvent(position));

    [JSInvokable]
    public void OnDraggingStarted(LatLng position)
        => this.Push(new DraggingStartEvent(position));

    [JSInvokable]
    public void OnDragging(LatLng position)
        => this.Push(new DraggingEvent(position));

    [JSInvokable]
    public void OnDraggingEnd(LatLng position)
        => this.Push(new DraggingEndEvent(position));

    [JSInvokable]
    public void OnDrop(LatLng position)
        => this.Push(new DropEvent(position));
}
