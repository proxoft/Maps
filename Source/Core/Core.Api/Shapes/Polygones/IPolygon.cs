namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public interface IPolygon : IShape
{
    public LatLngBounds GetBounds();

    public PolygonLatLng GetLatLngs();

    public void SetLatLngs(PolygonLatLng latLngs);
}
