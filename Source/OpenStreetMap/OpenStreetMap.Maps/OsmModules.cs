using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.OpenStreetMap.Maps.Infrastructure;

namespace Proxoft.Maps.OpenStreetMap.Maps;

internal class OsmModules
{
    private static readonly ValueOrWait<OsmModules> _modules = new(null!);
    private static bool _initialized;

    private OsmModules(
        IJSInProcessObjectReference map,
        IJSInProcessObjectReference marker,
        IJSInProcessObjectReference polygon,
        IJSInProcessObjectReference polyline,
        IJSInProcessObjectReference circle
        )
    {
        this.Map = map;
        this.Marker = marker;
        this.Polygon = polygon;
        this.Polyline = polyline;
        this.Circle = circle;
    }

    public IJSInProcessObjectReference Map { get; }

    public IJSInProcessObjectReference Marker { get; }

    public IJSInProcessObjectReference Polygon { get; }

    public IJSInProcessObjectReference Polyline { get; }

    public IJSInProcessObjectReference Circle { get; }

    public static IObservable<OsmModules> Load(IJSRuntime jsRuntime, string resourcePath)
    {
        if (!_initialized)
        {
            _initialized = true;

            string version = GetJsVersion();
            Console.WriteLine($"Loading scripts version: {version}");

            var mapS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $".{resourcePath}/map_{version}.js") // $"./_content/Proxoft.Maps.OpenStreetMap.Maps/maps_{v}.js")
                .AsTask();

            var markerS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $".{resourcePath}/marker_{version}.js")
                .AsTask();

            var polygonsS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $".{resourcePath}/polygon_{version}.js")
                .AsTask();

            var polylinesS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $".{resourcePath}/polyline_{version}.js")
                .AsTask();

            var circleS = jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                    "import",
                    $".{resourcePath}/circle_{version}.js")
                .AsTask();

            Task.WhenAll(mapS, markerS, polygonsS, polylinesS, circleS)
                .ToObservable()
                .Take(1)
                .Do(modules =>
                {
                    OsmModules osm = new(modules[0], modules[1], modules[2], modules[3], modules[4]);
                    _modules.SetValue(osm);
                })
                .Subscribe(
                    _ => { },
                    e => Console.WriteLine($"OsmModules.Load: {e.Message}")
                );
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
