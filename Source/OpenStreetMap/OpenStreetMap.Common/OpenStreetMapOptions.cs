namespace Proxoft.Maps.OpenStreetMap.Common;

public class OpenStreetMapOptions
{
    public string ResourcePath { get; set; } = "/openStreetMap";

    public string Language { get; set; } = "en";

    public bool ConsoleLogExceptions { get; set; } = true;

    public bool ConsoleTraceLogGeocoder { get; set; } = false;

    public int StreetGeometryMaxIterations { get; set; } = 8;
}
