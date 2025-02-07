using System;

namespace Proxoft.Maps.Core.Abstractions.Models;

public record LatLngBounds
{
    public static readonly LatLngBounds Empty = new();

    public decimal East { get; init; }

    public decimal West { get; init; }

    public decimal North { get; init; }

    public decimal South { get; init; }

    public LatLng SouthEast => new() { Latitude = this.South, Longitude = this.East };

    public LatLng SouthWest => new() {  Latitude = this.South, Longitude = this.West };

    public LatLng NorthEast => new() { Latitude = this.North, Longitude = this.East };

    public LatLng NorthWest => new() { Latitude = this.North, Longitude = this.West };

    public LatLng Center => new()
    {
        Latitude = this.South + (this.North - this.South) / 2,
        Longitude = this.East + (this.West - this.East) / 2
    };

    public bool Covers(LatLngBounds other)
    {
        return this.East >= other.East
            && this.North >= other.North
            && this.South <= other.South
            && this.West <= other.West;
    }

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
            North = Math.Max(corner1.Latitude, corner2.Latitude),
            South = Math.Min(corner1.Latitude, corner2.Latitude)
        };
    }

    public LatLngBounds MoveCenter(LatLng center)
    {
        decimal latWidth = (this.North - this.South) / 2;
        decimal lngWidth = (this.West - this.East) / 2;

        LatLng sw = new()
        {
            Latitude = center.Latitude - latWidth,
            Longitude = center.Longitude - lngWidth
        };

        LatLng ne = new()
        {
            Latitude = center.Latitude + latWidth,
            Longitude = center.Longitude + lngWidth
        };

        return FromCorners(sw, ne);
    }
}
