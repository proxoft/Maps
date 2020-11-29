using Proxoft.Extensions.ValueObjects.ValueObjects;

namespace Proxoft.Maps.Core.Api
{
    public class ZoomLevel : IntValueObject<ZoomLevel>
    {
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

        public ZoomLevel(int value) : base(value, 0, 18)
        {
        }
    }
}
