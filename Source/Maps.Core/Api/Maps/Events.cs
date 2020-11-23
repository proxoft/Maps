namespace Proxoft.Maps.Core.Api.Maps.Events
{
    public class CenterChangedEvent : Event<LatLng>
    {
        public CenterChangedEvent(LatLng value, EventSource source = EventSource.Js) : base(value, source)
        {
        }
    }

    public class ZoomChanged : Event<int>
    {
        public ZoomChanged(int value, EventSource source = EventSource.Js) : base(value, source)
        {
        }
    }
}
