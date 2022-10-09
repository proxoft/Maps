using System.Drawing;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.StaticMaps;

public class MarkerOptions
{
    public LatLng LatLng { get; set; }

    public string Label { get; set; }

    public Color Color { get; set; }
}
