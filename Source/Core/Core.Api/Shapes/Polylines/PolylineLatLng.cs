using System;
using System.Collections.Generic;
using System.Linq;

namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public class PolylineLatLng
{
    public static readonly PolylineLatLng Empty = new();

    public LatLng[][] LatLngs { get; set; } = Array.Empty<LatLng[]>();

    public static PolylineLatLng SingleLine(params LatLng[] latlngs)
    {
        return new PolylineLatLng()
        {
            LatLngs = new LatLng[][] { latlngs }
        };
    }

    public static PolylineLatLng MultiLine(IEnumerable<LatLng[]> linesLatLngs)
    {
        return new PolylineLatLng()
        {
            LatLngs = linesLatLngs.ToArray()
        };
    }
}
