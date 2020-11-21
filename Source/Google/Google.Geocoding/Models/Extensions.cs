using System.Linq;

namespace Proxoft.Maps.Google.Geocoding
{
    internal static class Extensions
    {
        public static string GetLongNameOfType(this GeocoderResult result, params string[] types)
        {
            var ac = result
                .address_components
                .FirstOrDefault(a => a.types.Any(t => types.Any(p => p == t)));

            return ac?.long_name;
        }
    }
}
