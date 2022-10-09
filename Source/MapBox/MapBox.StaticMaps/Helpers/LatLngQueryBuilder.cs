using System;
using Proxoft.Maps.Core;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.MapBox.StaticMaps.Helpers;

internal static class LatLngQueryBuilder
{
    public static string ToQueryParameter(this LatLng latLng)
        => FormattableString.Invariant($"{latLng.Longitude},{latLng.Latitude}");
}
