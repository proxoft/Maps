using Proxoft.Extensions.ValueObjects.ValueObjects;

namespace Proxoft.Maps.Core.Api
{
    public class Opacity : DecimalValueObject<Opacity>
    {
        public static readonly Opacity Visible = new (1);
        public static readonly Opacity Invisible = new (0);

        public Opacity(decimal value) : base(value, 0, 1)
        {
        }

        public static Opacity From(bool visible)
            => visible ? Visible : Invisible;
    }
}
