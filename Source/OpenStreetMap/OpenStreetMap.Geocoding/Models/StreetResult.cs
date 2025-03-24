namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class StreetResult
{
    public long place_id { get; set; }

    public string osm_type { get; set; } = "";

    public long osm_id { get; set; }

    public Geometry geometry { get; set; } = new();
}
