namespace Proxoft.Maps.Core.Api;

public class MouseClickEvent : Event<LatLng>
{
    public MouseClickEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseDoubleClickEvent : Event<LatLng>
{
    public MouseDoubleClickEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseDownEvent : Event<LatLng>
{
    public MouseDownEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseUpEvent : Event<LatLng>
{
    public MouseUpEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseEnterEvent : Event<LatLng>
{
    public MouseEnterEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseMoveEvent : Event<LatLng>
{
    public MouseMoveEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}

public class MouseLeaveEvent : Event<LatLng>
{
    public MouseLeaveEvent(LatLng value) : base(value, EventSource.Js)
    {
    }
}
