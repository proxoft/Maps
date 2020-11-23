namespace Proxoft.Maps.Core.Api
{
    public record MapOptions
    {
        public LatLng Center { get; set; }
        public int Zoom { get; set; }
    }
}
