using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxoft.Maps.Core.Api.Shapes;

public interface IPolygon : IApiObject
{
    public LatLngBounds GetBounds();

    public PolygonLatLng GetLatLngs();

    public void SetLatLng(PolygonLatLng latLngs);

    public void SetStyle(Style style);
}
