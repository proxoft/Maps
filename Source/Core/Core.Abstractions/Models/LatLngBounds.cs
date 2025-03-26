using System;
using System.Linq;

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

    public static LatLngBounds FromPositions(params LatLng[] positions)
    {
        if(positions.Length == 0)
        {
            return LatLngBounds.Empty;
        }

        decimal north = positions.Select(l => l.Latitude).Max();
        decimal south = positions.Select(l => l.Latitude).Min();
        decimal east = positions.Select(l => l.Longitude).Max();
        decimal west = positions.Select(l => l.Longitude).Min();

        return LatLngBounds.FromCorners(new LatLng()
            {
                Latitude = north,
                Longitude = west,
            },
            new LatLng()
            {
                Latitude = south,
                Longitude = east,
            }
        );
    }

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
