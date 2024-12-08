namespace Proxoft.Maps.OpenStreetMap.Maps;

public class IdFactory : IIdFactory
{
    private int _id = 0;

    public string NextMapId()
    {
        return this.NextId("map");
    }

    public string NextMarkerId()
    {
        return this.NextId("mrk");
    }

    public string NextPolygonId()
    {
        return this.NextId("plg");
    }

    public string NextPolylineId()
    {
        return this.NextId("pll");
    }

    public string NextCircleId()
    {
        return this.NextId("crc");
    }

    private string NextId(string prefix, string separator = "-")
    {
        int id = this.NextId();
        return $"{prefix}{separator}{id}";
    }

    private int NextId()
    {
        return ++_id;
    }
}
