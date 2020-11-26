using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding
{
    public interface IOsmResultParser
    {
        Core.Geocoding.Address Parse(Result result);
    }
}
