namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class GeocodeResult
{
    public int place_rank { get; set; }

    public AddressDetail address { get; set; } = new();

    public string lat { get; set; } = "";

    public string lon { get; set; } = "";

    public long osm_id { get; set; }

    public string osm_type {get; set; } = "";
}
