using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.OpenStreetMap.Maps.Infrastructure;

namespace Proxoft.Maps.OpenStreetMap.Maps;

internal class OsmModules
{
    private static ValueOrWait<OsmModules> _modules = new(null!);
    private static bool _initialized;

    private OsmModules(
        IJSInProcessObjectReference map,
        IJSInProcessObjectReference marker,
        IJSInProcessObjectReference polygon
        )
    {
        this.Map = map;
        this.Marker = marker;
        this.Polygon = polygon;
    }

    public IJSInProcessObjectReference Map { get; }

    public IJSInProcessObjectReference Marker { get; }

    public IJSInProcessObjectReference Polygon { get; }

    public static IObservable<OsmModules> Load(IJSRuntime jsRuntime)
    {
        if (!_initialized)
        {
            _initialized = true;

            string v = GetJsVersion();
            Console.WriteLine($"Loading scripts version: {v}");

            var mapS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $"./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_{v}.js")
                .AsTask();

            var markerS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $"./_content/Proxoft.Maps.OpenStreetMap.Maps/marker_{v}.js")
                .AsTask();

            var polygonsS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $"./_content/Proxoft.Maps.OpenStreetMap.Maps/polygon_{v}.js")
                .AsTask();

            Task.WhenAll(mapS, markerS, polygonsS)
                .ToObservable()
                .Take(1)
                .Subscribe(tripple =>
                {
                    OsmModules osm = new(tripple[0], tripple[1], tripple[2]);
                    _modules.SetValue(osm);
                });
        }

        return _modules;
    }

    private static string GetJsVersion()
    {
        string v = typeof(OsmModules).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        int i = v.LastIndexOf('.');
        return v[..i];
    }
}
