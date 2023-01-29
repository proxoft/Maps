using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal class OsmPolygon : Polygon
{
    public OsmPolygon(
        string id,
        Action<string> onRemove,
        IJSInProcessObjectReference jsModule
        ) : base(id, onRemove, jsModule)
    {
    }
}
