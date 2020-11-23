using System;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.Core.Api
{
    public static class IMapExtensions
    {
        public static IObservable<LatLng> OnCenter(this IMap map)
            => map.OnEvent.OfType<CenterChangedEvent>().Select(e => e.Value);

        public static IObservable<LatLng> OnCenter(this IMap map, Func<CenterChangedEvent, bool> filter)
            => map.OnEvent
                .OfType<CenterChangedEvent>()
                .Where(e => filter(e))
                .Select(e => e.Value);

        public static IObservable<int> OnZoom(this IMap map)
            => map.OnEvent
                .OfType<ZoomChanged>()
                .Select(e => e.Value);

        public static IObservable<int> OnZoom(this IMap map, Func<ZoomChanged, bool> filter)
            => map.OnEvent
                .OfType<ZoomChanged>()
                .Where(e => filter(e))
                .Select(e => e.Value);

        public static IObservable<LatLng> OnClick(this IMap map)
            => map.OnEvent
                .OfType<MapClickEvent>()
                .Select(e => e.Value);
    }
}
