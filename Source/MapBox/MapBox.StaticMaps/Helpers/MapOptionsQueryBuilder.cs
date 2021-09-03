using System;
using Proxoft.Maps.Core.StaticMaps;

namespace Proxoft.Maps.MapBox.StaticMaps.Helpers
{
    internal static class MapOptionsQueryBuilder
    {
        public static string ToQueryParameter(this MapOptions mapOptions)
        {
            var zoom = FormattableString.Invariant($"{mapOptions.Zoom}");
            return $"{mapOptions.Center.ToQueryParameter()},{zoom},0/{mapOptions.Size.Width}x{mapOptions.Size.Height}";
        }
    }
}
