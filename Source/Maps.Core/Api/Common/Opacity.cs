using System;

namespace Proxoft.Maps.Core.Api
{
    public record Opacity
    {
        public static readonly Opacity Visible = new (1);
        public static readonly Opacity Invisible = new (0);

        public Opacity(decimal value)
        {
            if(value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between 0 and 1");
            }

            this.Value = value;
        }

        public decimal Value { get; }

        public static Opacity From(bool visible)
            => visible ? Visible : Invisible;
    }
}
