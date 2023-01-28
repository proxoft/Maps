using System;

namespace Proxoft.Maps.Core.Abstractions.Models;

public class ZoomLevel: IEquatable<ZoomLevel>
{
    public static readonly ZoomLevel Default = new(-1);
    public static readonly ZoomLevel Zero = new (0);
    public static readonly ZoomLevel One = new (1);
    public static readonly ZoomLevel Two = new (2);
    public static readonly ZoomLevel Three = new (3);
    public static readonly ZoomLevel Four = new (4);
    public static readonly ZoomLevel Five = new (5);
    public static readonly ZoomLevel Six = new (6);
    public static readonly ZoomLevel Seven = new (7);
    public static readonly ZoomLevel Eight = new (8);
    public static readonly ZoomLevel Nine = new (9);
    public static readonly ZoomLevel Ten = new (10);
    public static readonly ZoomLevel Eleven = new (11);
    public static readonly ZoomLevel Twelve = new (12);
    public static readonly ZoomLevel Thirteen = new (13);
    public static readonly ZoomLevel Fourteen = new (14);
    public static readonly ZoomLevel Fifteen = new (15);
    public static readonly ZoomLevel Sixteen = new (16);
    public static readonly ZoomLevel Seventeen = new (17);
    public static readonly ZoomLevel Eighteen = new (18);

    private readonly decimal _value;

    public ZoomLevel(decimal value)
    {
        if(_value != -1 && _value < 0 || _value > 18)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 18");
        }

        _value = value;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as ZoomLevel);
    }

    public bool Equals(ZoomLevel? other)
    {
        return other is not null && other._value == _value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public static bool operator ==(ZoomLevel left, ZoomLevel right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ZoomLevel left, ZoomLevel right)
    {
        return !(left == right);
    }

    public static bool operator >(ZoomLevel left, ZoomLevel right)
    {
        return left._value > right._value;
    }

    public static bool operator <(ZoomLevel left, ZoomLevel right)
    {
        return left._value < right._value;
    }

    public static implicit operator decimal(ZoomLevel zoomLevel) => zoomLevel._value;

    public static explicit operator ZoomLevel(decimal value) => new(value);
}
