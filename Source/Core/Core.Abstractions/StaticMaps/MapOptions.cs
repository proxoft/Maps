using System.Collections.Generic;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.StaticMaps;

public class MapOptions
{
    public LatLng Center { get; set; } = LatLng.None;

    public SizePixel Size { get; set; } = new() { Width = 100, Height = 100 };

    public ZoomLevel Zoom { get; set; } = ZoomLevel.Default;

    public IReadOnlyCollection<MarkerOptions> Markers { get; set; } = [];
}
