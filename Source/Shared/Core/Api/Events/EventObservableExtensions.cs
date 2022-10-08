using System;
using System.Reactive.Linq;

namespace Proxoft.Maps.Core.Api
{
    internal static class EventObservableExtensions
    {
        public static IObservable<F> Filter<F>(this IObservable<Event> source, Func<F, bool> filter = null)
            where F : Event
        {
            return source
                .OfType<F>()
                .WhereEx(filter);
        }

        private static IObservable<E> WhereEx<E>(this IObservable<E> source, Func<E, bool> filter)
        {
            return filter == null
                ? source
                : source.Where(e => filter(e));
        }
    }
}
