using System;
using System.Linq;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.Geocoding;

public record StreetGeometry
{
    private int _hashCode = 0;

    public StreetLine[] Lines { get; init; } = [];

    public virtual bool Equals(StreetGeometry? other)
    {
        if(other is null)
        {
            return false;
        }

        return this.Lines.SequenceEqual(other.Lines);
    }

    public override int GetHashCode()
    {
        if(_hashCode == 0)
        {
            _hashCode = this.Lines.CalculateHashCode();
        }

        return _hashCode;
    }
}

public record StreetLine
{
    private int _hashCode = 0;

    public LatLng[] Points { get; init; } = [];

    public virtual bool Equals(StreetLine? other)
    {
        if(other is null)
        {
            return false;
        }

        return this.Points.SequenceEqual(other.Points);
    }

    public override int GetHashCode()
    {
        if(_hashCode == 0)
        {
            _hashCode = this.Points.CalculateHashCode();
        }

        return  _hashCode;
    }
}
