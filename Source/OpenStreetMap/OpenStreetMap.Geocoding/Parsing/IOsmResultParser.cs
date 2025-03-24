using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

public interface IOsmResultParser
{
    Core.Abstractions.Geocoding.Address Parse(Result result);
}
