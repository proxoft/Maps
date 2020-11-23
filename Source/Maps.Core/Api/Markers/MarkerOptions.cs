namespace Proxoft.Maps.Core.Api
{
    public record MarkerOptions
    {
        public LatLng Position { get; set; }
        public bool Draggable { get; set; } = true;
    }
}
