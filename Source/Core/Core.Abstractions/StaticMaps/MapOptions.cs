using System.Collections.Generic;
using Proxoft.Maps.Core.Abstractions.Common;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.StaticMaps;

public class MapOptions
{
    public LatLng Center { get; set; }

    public SizePixel Size { get; set; }

    public ZoomLevel Zoom { get; set; }

    public IReadOnlyCollection<MarkerOptions> Markers { get; set; }
}
