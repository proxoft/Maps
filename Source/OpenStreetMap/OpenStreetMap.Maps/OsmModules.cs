using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Collections.Generic;
using Proxoft.Maps.OpenStreetMap.Maps.Infrastructure;

namespace Proxoft.Maps.OpenStreetMap.Maps;

internal class OsmModules
{
    private static readonly ValueOrWait<OsmModules> _modules = ValueOrWait<OsmModules>.Empty();
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
            string path = resourcePath.NormalizePath();
            string version = VersionProvider.GetScriptsVersion();

            Console.WriteLine($"Loading OpenStreetMap scripts version: {version}");

            PrepareImportTasks(jsRuntime, path, version)
                .WhenAll()
                .ToObservable()
                .Take(1)
                .Select(modules => modules.Skip(2).ToArray())
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

    private static IEnumerable<Task<IJSInProcessObjectReference>> PrepareImportTasks(
        IJSRuntime jsRuntime, string resourcePath, string version)
    {
        yield return jsRuntime.ImportCss($"{resourcePath}/leafletCss.js", resourcePath);
        yield return jsRuntime.ImportScript($"{resourcePath}/leaflet.js");
        yield return jsRuntime.ImportScript($"{resourcePath}/map.{version}.js");
        yield return jsRuntime.ImportScript($"{resourcePath}/marker.{version}.js");
        yield return jsRuntime.ImportScript($"{resourcePath}/polygon.{version}.js");
        yield return jsRuntime.ImportScript($"{resourcePath}/polyline.{version}.js");
        yield return jsRuntime.ImportScript($"{resourcePath}/circle.{version}.js");
    }
}

file static class JsRuntimeExtensions
{
    public static Task<IJSInProcessObjectReference> ImportScript(
        this IJSRuntime jsRuntime,
        string scriptPath)
    {
        return jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
            "import",
            scriptPath)
            .AsTask();
    }

    public static async Task<IJSInProcessObjectReference> ImportCss(
        this IJSRuntime jsRuntime,
        string scriptPath,
        string resourcePath)
    {
        IJSInProcessObjectReference jsObjectReference = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
            "import",
            scriptPath);

        await jsObjectReference.InvokeVoidAsync("appendOpenStreetMapCss", resourcePath);
        return jsObjectReference;
    }
}

file static class TaskExtensions
{
    public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        return Task.WhenAll(tasks);
    }
}

file static class StringExtensions
{
    public static string NormalizePath(this string resourcePath)
    {
        string result = resourcePath;
        if (resourcePath.EndsWith('/'))
        {
            result = result[..^1];
        }

        return result;
    }
}