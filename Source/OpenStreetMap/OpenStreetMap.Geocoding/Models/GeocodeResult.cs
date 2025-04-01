namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class GeocodeResult
{
    public long place_id { get; set; }

    public int place_rank { get; set; }

    public string category { get; set; } = "";

    public AddressDetail address { get; set; } = new();

    public string lat { get; set; } = "";

    public string lon { get; set; } = "";

    public long osm_id { get; set; }

    public string osm_type {get; set; } = "";

    public Geometry geojson { get; set; } = new();
}
