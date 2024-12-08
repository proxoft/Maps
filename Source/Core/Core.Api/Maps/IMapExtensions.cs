using System;
using System.Drawing;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api.Maps;

public static class IMapExtensions
{
    public static IObservable<Size> OnResized(this IMap map, Func<ResizedEvent, bool>? filter = null)
        => map.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnCenterChanged(this IMap map, Func<CenterChangedEvent, bool>? filter = null)
        => map.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<ZoomLevel> OnZoomed(this IMap map, Func<ZoomChanged, bool>? filter = null)
        => map.OnEvent
            .Filter(filter)
            .Select(e => e.Value);
}
