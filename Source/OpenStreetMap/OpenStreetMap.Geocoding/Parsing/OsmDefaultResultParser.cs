using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

public class OsmDefaultResultParser : IOsmResultParser
{
    public Core.Abstractions.Geocoding.Address Parse(Result result)
        => result.ToAddress();
}
