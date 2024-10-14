namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public interface IPolyline : IApiObject
{
    public LatLngBounds GetBounds();

    public PolylineLatLng GetLatLngs();

    public void SetLatLngs(LatLng[] latLngs);

    public void SetLatLngs(LatLng[][] latLngs);

    public void SetStyle(Style style);
}
