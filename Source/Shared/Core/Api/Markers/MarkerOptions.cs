namespace Proxoft.Maps.Core.Api
{
    public record MarkerOptions
    {
        public LatLng Position { get; set; }
        public bool Draggable { get; set; } = true;
        public Opacity Opacity { get; set; } = Opacity.Visible;
        /// <summary>
        /// Logs JS activity in console
        /// </summary>
        public bool TraceJs { get; set; }
    }
}
