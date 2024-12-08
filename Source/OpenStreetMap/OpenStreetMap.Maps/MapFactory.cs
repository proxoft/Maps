using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Maps.Initialization;
using Proxoft.Maps.OpenStreetMap.Maps.Models;
using Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps;

public class MapFactory : IMapFactory
{
    private readonly ApiLoader _api;
    private readonly IJSRuntime _jsRuntime;
    private readonly OpenStreetMapOptions _options;
    private readonly IIdFactory _idFactory;

    public MapFactory(
        IIdFactory idFactory,
        IJSRuntime jsRuntime,
        OpenStreetMapOptions options)
    {
        _api = new ApiLoader(jsRuntime);
        _idFactory = idFactory;
        _jsRuntime = jsRuntime;
        this._options = options;
    }

    public string Name => "OpenStreetMaps";

    public IObservable<IMap> Initialize(
        MapOptions options,
        ElementReference hostElement)
    {
        return _api.LoadMapScripts()
            .ToObservable()
            .Select(response =>
            {
                return response == LoadResponse.Loaded
                    ? this.CreateMap(options, hostElement)
                    : Observable.Return<IMap>(NoMap.Instance);
            })
            .Switch()
            .Take(1);
    }

    private IObservable<IMap> CreateMap(MapOptions options, ElementReference hostElement)
    {
        Console.WriteLine("creating map");

        return OsmModules.Load(_jsRuntime, _options.ResourcePath)
            .Select(modules => {
                Console.WriteLine("modules loaded");

                OsmMapObjectsFactory mapObjectsFactory = new(_idFactory, modules);
                Map map = mapObjectsFactory.CreateMap(options, hostElement);
                return map;
            });
    }
}
