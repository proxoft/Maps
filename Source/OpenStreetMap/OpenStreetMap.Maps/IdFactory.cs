using System.IO.Pipes;
using Proxoft.Maps.OpenStreetMap;

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
        return this.NextId("pol");
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
