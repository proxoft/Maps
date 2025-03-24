using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.Geocoding;

public record StreetGeometry
{
    public static readonly StreetGeometry Empty = new();

    public LatLng[] Coordinates { get; init; } = [];

    public StreetLine[] Lines { get; init; } = [];
}

public record StreetLine(LatLng[] Nodes)
{
    public static readonly StreetLine Empty = new([]);
}
