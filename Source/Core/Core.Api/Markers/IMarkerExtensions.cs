using System;
using System.Linq;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api;

public static class IMarkerExtensions
{
    public static IObservable<LatLng> OnPositionStartChange(this IMarker marker, Func<PositionStartChangeEvent, bool> filter = null)
        => marker.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnPositionChanging(this IMarker marker, Func<PositionChangingEvent, bool> filter = null)
        => marker.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnPositionChanged(this IMarker marker, Func<PositionChangedEvent, bool> filter = null)
        => marker.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    //public static IObservable<LatLng> OnDragStart(this IMarker marker, Func<DragStartEvent, bool> filter)
    //    => marker.OnEvent
    //        .Filter(filter)
    //        .Select(e => e.Value);

    //public static IObservable<LatLng> OnDragging(this IMarker marker, Func<DraggingEvent, bool> filter)
    //    => marker.OnEvent
    //        .Filter(filter)
    //        .Select(e => e.Value);

    //public static IObservable<LatLng> OnDragEnd(this IMarker marker, Func<DragEndEvent, bool> filter)
    //    => marker.OnEvent
    //        .Filter(filter)
    //        .Select(e => e.Value);
}
