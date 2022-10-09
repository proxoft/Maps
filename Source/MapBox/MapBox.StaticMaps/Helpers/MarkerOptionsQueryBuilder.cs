using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Proxoft.Maps.Core.Abstractions.StaticMaps;

namespace Proxoft.Maps.MapBox.StaticMaps.Helpers;

internal static class MarkerOptionsQueryBuilder
{
    public static string ToQueryParameter(this IReadOnlyCollection<MarkerOptions> markers)
    {
        if((markers?.Count ?? 0) == 0)
        {
            return string.Empty;
        }

        var ms = markers
            .Select(m => m.ToQueryParameter())
            .ToArray();

        return string.Join(",", ms) + "/";
    }

    private static string ToQueryParameter(this MarkerOptions marker)
    {
        var result = "pin-s" + marker.Label.ToLabelQueryParameter();
        
        if(marker.Color != Color.Empty)
        {
            result += $"+{marker.Color.ToHex()}";
        }

        return result + $"({marker.LatLng.ToQueryParameter()})";
    }

    private static string ToLabelQueryParameter(this string label)
    {
        var l = label.ExtractValidLabel();

        return string.IsNullOrWhiteSpace(l)
            ? string.Empty
            : $"-{l.ToLower()}";
    }

    private static string ToHex(this Color color)
        => $"{color.R:X2}{color.G:X2}{color.B:X2}";

    private static string ExtractValidLabel(this string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return string.Empty;
        }

        char c = label[0];
        if (c.IsBasicLetter())
        {
            return c.ToString();
        }

        if (!char.IsNumber(c))
        {
            return string.Empty;
        }

        if(label.Length == 1)
        {
            return c.ToString();
        }

        var c2 = label[1];
        return char.IsNumber(c2)
            ? (c + c2).ToString()
            :  c.ToString();
    }

    private static bool IsBasicLetter(this char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
}
