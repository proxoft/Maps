using System;
using System.Drawing;
using System.Reactive.Linq;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.Core.Api
{
    public static class IMapExtensions
    {
        public static IObservable<Size> OnResized(this IMap map, Func<ResizedEvent, bool> filter = null)
            => map.OnEvent
                .Filter(filter)
                .Select(e => e.Value);

        public static IObservable<LatLng> OnCenter(this IMap map, Func<CenterChangedEvent, bool> filter = null)
            => map.OnEvent
                .Filter(filter)
                .Select(e => e.Value);

        public static IObservable<ZoomLevel> OnZoom(this IMap map, Func<ZoomChanged, bool> filter = null)
            => map.OnEvent
                .Filter(filter)
                .Select(e => e.Value);
    }
}
