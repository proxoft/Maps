using System;
using System.Text.RegularExpressions;

namespace Proxoft.Maps.Core.Abstractions.Models;

public record LatLngBounds
{
    public static readonly LatLngBounds Empty = new();

    public decimal East { get; init; }

    public decimal West { get; init; }

    public decimal North { get; init; }

    public decimal South { get; init; }

    public LatLng SouthWest() => new() {  Latitude = this.South, Longitude = this.West };

    public LatLng NorthEast() => new() { Latitude = this.North, Longitude = this.East };

    public static LatLngBounds FromPosition(LatLng position)
        => new()
        {
            East = position.Longitude,
            West = position.Longitude,
            North= position.Latitude,
            South= position.Latitude,
        };

    public static LatLngBounds FromCorners(LatLng corner1, LatLng corner2)
    {
        return new()
        {
            East = Math.Max(corner1.Longitude, corner2.Longitude),
            West = Math.Min(corner1.Longitude, corner2.Longitude),
            North = Math.Min(corner1.Latitude, corner2.Latitude),
            South = Math.Max(corner1.Latitude, corner2.Latitude)
        };
    }
}
