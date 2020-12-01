using System.Collections.Generic;

namespace Proxoft.Maps.Core.StaticMaps
{
    public class MapOptions
    {
        public LatLng Center { get; set; }
        public SizePixel Size { get; set; }
        public ZoomLevel Zoom { get; set; }
        public IReadOnlyCollection<MarkerOptions> Markers { get; set; }
    }
}
