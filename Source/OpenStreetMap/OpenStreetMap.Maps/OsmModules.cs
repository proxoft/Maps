using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Proxoft.Maps.OpenStreetMap.Maps;

internal class OsmModules
{
    private OsmModules(
        IJSInProcessObjectReference map,
        IJSInProcessObjectReference marker,
        IJSInProcessObjectReference polygon
        )
    {
        Map = map;
        Marker = marker;
        Polygon = polygon;
    }

    public IJSInProcessObjectReference Map { get; }

    public IJSInProcessObjectReference Marker { get; }

    public IJSInProcessObjectReference Polygon { get; }

    public static async Task<OsmModules> Load(IJSRuntime jsRuntime)
    {
        var map = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_0.5.0.js");

        var marker = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Proxoft.Maps.OpenStreetMap.Maps/marker_0.5.0.js");

        var polygons = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Proxoft.Maps.OpenStreetMap.Maps/polygon_0.5.0.js");

        return new OsmModules(map, marker, polygons);
    }
}
