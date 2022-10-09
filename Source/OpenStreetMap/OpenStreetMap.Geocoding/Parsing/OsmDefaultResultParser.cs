using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

namespace Proxoft.Maps.OpenStreetMap.Geocoding
{
    public class OsmDefaultResultParser : IOsmResultParser
    {
        public Core.Abstractions.Geocoding.Address Parse(Result result)
            => result.ToAddress();
    }
}
