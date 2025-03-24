using System;
using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Maps.Models;

namespace Proxoft.Maps.OpenStreetMap.Maps;

public class MapFactory(
    IIdFactory idFactory,
    IJSRuntime jsRuntime,
    OpenStreetMapOptions options) : IMapFactory
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly OpenStreetMapOptions _options = options;
    private readonly IIdFactory _idFactory = idFactory;

    public string Name => "OpenStreetMaps";

    public IObservable<IMap> Initialize(
        MapOptions options,
        ElementReference hostElement)
    {
        return this.CreateMap(options, hostElement);
    }

    private IObservable<IMap> CreateMap(MapOptions options, ElementReference hostElement)
    {
        return OsmModules.Load(_jsRuntime, _options.ResourcePath)
            .Select(modules => {
                OsmMapObjectsFactory mapObjectsFactory = new(_idFactory, modules);
                Map map = mapObjectsFactory.CreateMap(options, hostElement);
                return map;
            })
            .Take(1);
    }
}
