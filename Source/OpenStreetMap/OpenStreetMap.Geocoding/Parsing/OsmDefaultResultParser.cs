using System.Linq;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;
using Proxoft.Optional;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

public class OsmDefaultResultParser : IOsmResultParser
{
    public Either<ErrorStatus, Address> ParseAddress(params GeocodeResult[] result)
    {
        if(result.Length == 0)
        {
            return ErrorStatus.ZeroResults;
        }

        try
        {
            GeocodeResult geocodeResult = result.OrderByDescending(r => r.place_rank).First();
            return geocodeResult.ToAddress();
        }
        catch
        {
            return ErrorStatus.UnknownError;
        }
    }

    public Either<ErrorStatus, StreetGeometry> Parse(GeocodeResult[] results)
    {
        if(results.Length == 0)
        {
            return ErrorStatus.ZeroResults;
        }

        try
        {
            StreetLine[] lines = [
                ..results
                    .Select(r => r.geojson)
                    .OfType<LineGeometry>()
                    .Select(g => g.coordinates.ToStreetLine())
            ];

            return new StreetGeometry()
            {
                Lines = lines
            };
        }
        catch
        {
            return ErrorStatus.UnknownError;
        }
    }
}

file static class Operators
{
    public static StreetLine ToStreetLine(this decimal[][] coordinates) =>
        new()
        {
            Points = [..
                coordinates.Select(c => LatLng.From(lng: c[0], lat: c[1]))
            ]
        };
}
