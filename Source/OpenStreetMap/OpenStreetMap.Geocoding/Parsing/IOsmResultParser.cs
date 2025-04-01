using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
using Proxoft.Optional;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

public interface IOsmResultParser
{
    Either<ErrorStatus, Address> ParseAddress(params GeocodeResult[] result);

    Either<ErrorStatus, StreetGeometry> Parse(params GeocodeResult[] results);

    // Either<ErrorStatus, StreetGeometry> Parse(StreetResult[] streetResults);
}
