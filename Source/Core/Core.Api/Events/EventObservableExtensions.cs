using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api;

internal static class EventObservableExtensions
{
    public static IObservable<TEvent> Filter<TEvent>(this IObservable<Event> source, Func<TEvent, bool>? filter)
        where TEvent : Event
    {
        IObservable<TEvent> s = source.OfType<TEvent>();

        return filter is null
            ? s
            : s.Where(filter);
    }
}
