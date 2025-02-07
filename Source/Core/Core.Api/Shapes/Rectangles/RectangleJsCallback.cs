using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

internal sealed class RectangleJsCallback(Action<Event> onEvent) : ApiObjectJsCallback<RectangleJsCallback>(onEvent)
{
    [JSInvokable]
    public void OnDraggingStarted(LatLng[] corners)
    {
        LatLngBounds bounds = LatLngBounds.FromCorners(corners[0], corners[1]);
        this.Push(new RectangleDraggingStartEvent(bounds));
    }

    [JSInvokable]
    public void OnDragging(LatLng[] corners)
    {
        LatLngBounds bounds = LatLngBounds.FromCorners(corners[0], corners[1]);
        this.Push(new RectangleDraggingEvent(bounds));
    }

    [JSInvokable]
    public void OnDraggingEnded(LatLng[] corners)
    {
        LatLngBounds bounds = LatLngBounds.FromCorners(corners[0], corners[1]);
        this.Push(new RectangleDraggingEndEvent(bounds));
    }
}
