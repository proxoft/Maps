namespace Proxoft.Maps.Core.Api;

public class MouseClickEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseDoubleClickEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseDownEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseUpEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseEnterEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseMoveEvent(LatLng value) : Event<LatLng>(value)
{
}

public class MouseLeaveEvent(LatLng value) : Event<LatLng>(value)
{
}
