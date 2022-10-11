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
    public void OnPositionStartChange(LatLng position)
        => this.Push(new PositionStartChangeEvent(position));

    [JSInvokable]
    public void OnPositionChanging(LatLng position)
        => this.Push(new PositionChangingEvent(position));

    [JSInvokable]
    public void OnPositionChanged(LatLng position)
        => this.Push(new PositionChangedEvent(position));

    //[JSInvokable]
    //public void OnDragStart(LatLng position)
    //    => this.Push(new DragStartEvent(position));

    //[JSInvokable]
    //public void OnDraggging(LatLng position)
    //    => this.Push(new DraggingEvent(position));

    //[JSInvokable]
    //public void OnDragEnd(LatLng position)
    //    => this.Push(new DragEndEvent(position));
}
