namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

public interface IPolygon : IApiObject
{
    public LatLngBounds GetBounds();

    public PolygonLatLng GetLatLngs();

    public void SetLatLng(PolygonLatLng latLngs);

    public void SetStyle(Style style);
}
