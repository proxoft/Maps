using System;
using System.Collections.Generic;
using System.Linq;

namespace Proxoft.Maps.Core.Api.Maps;

internal static class EnumerableExtensions
{
    public static void DisposeAll<T>(this IEnumerable<T> source)
        where T: IDisposable
    {
        foreach(var i in source.ToArray())
        {
            i.Dispose();
        }
    }
}
