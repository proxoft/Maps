using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;

internal class OsmModules
{
    private OsmModules(
        IJSInProcessObjectReference map,
        IJSInProcessObjectReference marker
        )
    {
        this.Map = map;
        this.Marker = marker;
    }

    public IJSInProcessObjectReference Map { get; }

    public IJSInProcessObjectReference Marker { get; }

    public static async Task<OsmModules> Load(IJSRuntime jsRuntime)
    {
        var map = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_0.2.0.js");

        var marker = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Proxoft.Maps.OpenStreetMap.Maps/marker_0.2.0.js");

        await Task.WhenAll(map.AsTask(), marker.AsTask());

        return new OsmModules(map.Result, marker.Result);
    }
}
