namespace Proxoft.Maps.Core.Api;

public abstract class MarkerLatLngEvent : Event<LatLng>
{
    protected MarkerLatLngEvent(LatLng value) : base(value)
    {
    }
}

public class PositionChangedEvent : MarkerLatLngEvent
{
    public PositionChangedEvent(LatLng value) : base(value)
    {
    }
}

public class DragEvent : MarkerLatLngEvent
{ 
    public DragEvent(LatLng value) : base(value)
    {
    }
}

public class DraggingStartEvent : MarkerLatLngEvent
{
    public DraggingStartEvent(LatLng value) : base(value)
    {
    }
}

public class DraggingEvent : MarkerLatLngEvent
{
    public DraggingEvent(LatLng value) : base(value)
    {
    }
}

public class DraggingEndEvent : MarkerLatLngEvent
{
    public DraggingEndEvent(LatLng value) : base(value)
    {
    }
}

public class DropEvent : MarkerLatLngEvent
{
    public DropEvent(LatLng value) : base(value)
    {
    }
}
