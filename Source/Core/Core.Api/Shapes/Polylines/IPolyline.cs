namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public interface IPolyline : IShape
{
    public LatLngBounds GetBounds();

    public PolylineLatLng GetLatLngs();

    public void SetLatLngs(LatLng[] latLngs);

    public void SetLatLngs(LatLng[][] latLngs);
}
