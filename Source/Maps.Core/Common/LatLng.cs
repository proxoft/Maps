namespace Proxoft.Maps.Core
{
    public record LatLng
    {
        public static readonly LatLng None = new ();

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
