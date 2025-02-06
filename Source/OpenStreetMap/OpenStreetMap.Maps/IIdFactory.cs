namespace Proxoft.Maps.OpenStreetMap.Maps;

public interface IIdFactory
{
    string NextMapId();

    string NextMarkerId();

    string NextPolygonId();

    string NextPolylineId();

    string NextCircleId();

    string NextRectangleId();
}
