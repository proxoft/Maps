using System;
using Proxoft.Maps.Core;

namespace Proxoft.Maps.MapBox.StaticMaps.Helpers
{
    internal static class LatLngQueryBuilder
    {
        public static string ToQueryParameter(this LatLng latLng)
            => FormattableString.Invariant($"{latLng.Latitude},{latLng.Longitude}");
    }
}
