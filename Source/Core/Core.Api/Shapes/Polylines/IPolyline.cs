namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public interface IPolyline : IApiObject
{
    public LatLngBounds GetBounds();

    public PolylineLatLng GetLatLngs();

    public void SetLatLng(LatLng[] latLngs);

    public void SetLatLng(LatLng[][] latLngs);

    public void SetStyle(Style style);
}
