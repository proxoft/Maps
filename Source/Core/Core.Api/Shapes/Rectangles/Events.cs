namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

public abstract class RectangleEvent : Event
{
}


public abstract class RectangleDragEvent(LatLngBounds bounds) : RectangleEvent
{
    public LatLngBounds Bounds { get; } = bounds;
}

public sealed class RectangleDraggingStartEvent(LatLngBounds bounds) : RectangleDragEvent(bounds)
{
}

public sealed class RectangleDraggingEvent(LatLngBounds bounds) : RectangleDragEvent(bounds)
{
}

public sealed class RectangleDraggingEndEvent(LatLngBounds bounds) : RectangleDragEvent(bounds)
{
}

