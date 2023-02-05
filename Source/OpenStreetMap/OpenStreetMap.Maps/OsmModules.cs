using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Schema;
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

    public static IObservable<OsmModules> Load(IJSRuntime jsRuntime)
    {
        string v = GetJsVersion();

        var mapS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                $"./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_{v}.js")
            .AsTask()
            .ToObservable();

        var markerS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                $"./_content/Proxoft.Maps.OpenStreetMap.Maps/marker_{v}.js")
            .AsTask()
            .ToObservable();

        var polygonsS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                $"./_content/Proxoft.Maps.OpenStreetMap.Maps/polygon_{v}.js")
            .AsTask()
            .ToObservable();

        return Observable
            .Zip(mapS, markerS, polygonsS, (map, marker, polygon) =>
        {
            return new OsmModules(map, marker, polygon);
        });
    }

    private static string GetJsVersion()
    {
        string v = typeof(OsmModules).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        int i = v.LastIndexOf('.');
        return v[..i];
    }
}
