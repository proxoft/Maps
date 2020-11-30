namespace Proxoft.Maps.Core.Api
{
    public record LatLng
    {
        public static readonly LatLng None = new ();

        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}
