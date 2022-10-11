using System;
using System.Linq;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api;

public static class IApiObjectExtensions
{
    public static IObservable<LatLng> OnClick(this IApiObject apiObject, Func<MouseClickEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnDoubleClick(this IApiObject apiObject, Func<MouseDoubleClickEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnMouseDown(this IApiObject apiObject, Func<MouseDownEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnMouseUp(this IApiObject apiObject, Func<MouseUpEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnMouseEnter(this IApiObject apiObject, Func<MouseEnterEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnMouseMove(this IApiObject apiObject, Func<MouseMoveEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);

    public static IObservable<LatLng> OnMouseLeave(this IApiObject apiObject, Func<MouseLeaveEvent, bool> filter = null)
        => apiObject.OnEvent
            .Filter(filter)
            .Select(e => e.Value);
}
