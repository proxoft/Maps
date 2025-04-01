using System.Text.Json.Serialization;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PointGeometry), "Point")]
[JsonDerivedType(typeof(LineGeometry), "LineString")]
[JsonDerivedType(typeof(PolygonGeometry), "Polygon")]
public class Geometry
{
    public string type { get; set; } = "";
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class PointGeometry : Geometry
{
    public decimal[] coordinates { get; set; } = [];
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class LineGeometry : Geometry
{
    public decimal[][] coordinates { get; set; } = [];
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class PolygonGeometry : Geometry
{
    public decimal[][][] coordinates { get; set; } = [];
}
