namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class Geometry
{
    public string type { get; set; } = "";

    public decimal[][] coordinates { get; set; } = [];
}
