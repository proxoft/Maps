using System;
using System.Collections.Generic;

namespace Proxoft.Maps.Core.Abstractions;

internal static class HashCodeExtensions
{
    public static int CalculateHashCode<T>(this IEnumerable<T> items)
    {
        HashCode hc = new();
        foreach (var item in items)
        {
            hc.Add(item);
        }

        return hc.ToHashCode();
    }
}
