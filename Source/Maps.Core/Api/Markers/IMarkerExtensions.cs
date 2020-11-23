using System;
using System.Linq;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api.Markers;

namespace Proxoft.Maps.Core.Api
{
    public static class IMarkerExtensions
    {
        public static IObservable<LatLng> OnPosition(this IMarker marker)
            => marker.OnEvent
                .OfType<MarkerPositionChangedEvent>()
                .Select(e => e.Value);

        public static IObservable<LatLng> OnPosition(this IMarker marker, Func<MarkerPositionChangedEvent, bool> filter)
            => marker.OnEvent
                .OfType<MarkerPositionChangedEvent>()
                .Where(e => filter(e))
                .Select(e => e.Value);
    }
}
