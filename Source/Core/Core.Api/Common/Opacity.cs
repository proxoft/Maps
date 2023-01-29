using System;

namespace Proxoft.Maps.Core.Api;

public class Opacity
{
    public static readonly Opacity Visible = new (1);
    public static readonly Opacity Invisible = new (0);
    private readonly decimal _value;

    public Opacity(decimal value)
    {
        if(value < 0 || value > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        _value = value;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Opacity);
    }

    public bool Equals(Opacity? other)
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

    public static bool operator ==(Opacity left, Opacity right)
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

    public static bool operator !=(Opacity left, Opacity right)
    {
        return !(left == right);
    }

    public static bool operator >(Opacity left, Opacity right)
    {
        return left._value > right._value;
    }

    public static bool operator <(Opacity left, Opacity right)
    {
        return left._value < right._value;
    }

    public static implicit operator decimal(Opacity opacity) => opacity._value;

    public static explicit operator Opacity(decimal value) => new(value);

    public static explicit operator Opacity(bool value) => From(value);

    public static Opacity From(bool visible)
        => visible ? Visible : Invisible;
}
