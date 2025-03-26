using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
using Proxoft.Optional;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

public interface IOsmResultParser
{
    Either<ErrorStatus, Address> Parse(GeocodeResult[] result);

    Address Parse(GeocodeResult result);

    Either<ErrorStatus, StreetGeometry> Parse(StreetResult[] streetResults);
}
