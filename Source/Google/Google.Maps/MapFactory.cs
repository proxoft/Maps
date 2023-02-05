using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api.Maps;
using Proxoft.Maps.Google.Common;
using Proxoft.Maps.Google.Maps.Initialization;
using Proxoft.Maps.Google.Maps.Models.Maps;

namespace Proxoft.Maps.Google.Maps;

public sealed class MapFactory : IMapFactory, IAsyncDisposable 
{
    private readonly Lazy<Task<IJSInProcessObjectReference>> _moduleTask;
    private readonly ApiLoader _api;
    private readonly GoogleApiConfiguration _configuration;

    public string Name => "GoogleMaps";

    public MapFactory(GoogleApiConfiguration configuration, IJSRuntime jsRuntime)
    {
        _api = new ApiLoader(jsRuntime);
        _configuration = configuration;

        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
           "import",
           "./_content/Proxoft.Maps.Google.Maps/maps.js").AsTask());
    }

    public IObservable<IMap> Initialize(MapOptions options, ElementReference hostElement)
    {
        return _api.LoadGoogleScripts(_configuration)
            .ToObservable()
            .Select(response => {
                return response == LoadResponse.Loaded
                    ? this.CreateMap(options, hostElement)
                    : Observable.Return<IMap>(NoMap.Instance);
            })
            .Switch();
    }

    public async ValueTask DisposeAsync()
    {
        await _api.DisposeAsync();
    }

    private IObservable<IMap> CreateMap(MapOptions options, ElementReference hostElement)
    {
        throw new Exception("not implmented");
    }
}
