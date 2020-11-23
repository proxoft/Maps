namespace Proxoft.Maps.Core.Api.Markers
{
    public class MarkerPositionChangedEvent : Event<LatLng>
    {
        public MarkerPositionChangedEvent(LatLng value, EventSource source = EventSource.Js) : base(value, source)
        {
        }
    }
}
