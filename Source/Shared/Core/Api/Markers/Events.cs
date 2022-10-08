namespace Proxoft.Maps.Core.Api.Markers
{
    public class PositionStartChangeEvent : Event<LatLng>
    {
        public PositionStartChangeEvent(LatLng value) : base(value)
        {
        }
    }

    public class PositionChangingEvent : Event<LatLng>
    {
        public PositionChangingEvent(LatLng value) : base(value)
        {
        }
    }

    public class PositionChangedEvent : Event<LatLng>
    {
        public PositionChangedEvent(LatLng value) : base(value)
        {
        }
    }

    //public class DragStartEvent : Event<LatLng>
    //{
    //    public DragStartEvent(LatLng value) : base(value)
    //    {
    //    }
    //}

    //public class DraggingEvent : Event<LatLng>
    //{
    //    public DraggingEvent(LatLng value) : base(value)
    //    {
    //    }
    //}

    //public class DragEndEvent : Event<LatLng>
    //{
    //    public DragEndEvent(LatLng value) : base(value)
    //    {
    //    }
    //}
}
