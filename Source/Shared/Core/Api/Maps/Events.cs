using System.Drawing;

namespace Proxoft.Maps.Core.Api.Maps
{
    public class LoadedEvent : Event
    {
        public LoadedEvent() : base(EventSource.Js)
        {
        }
    }

    public class ResizedEvent : Event<Size>
    {
        public ResizedEvent(Size value) : base(value)
        {
        }
    }

    public class CenterChangingEvent : Event<LatLng>
    {
        public CenterChangingEvent(LatLng value) : base(value)
        {
        }
    }

    public class CenterChangedEvent : Event<LatLng>
    {
        public CenterChangedEvent(LatLng value) : base(value)
        {
        }
    }

    public class ZoomChanging : Event<ZoomLevel>
    {
        public ZoomChanging(ZoomLevel value) : base(value)
        {
        }
    }

    public class ZoomChanged : Event<ZoomLevel>
    {
        public ZoomChanged(ZoomLevel value) : base(value)
        {
        }
    }
}
