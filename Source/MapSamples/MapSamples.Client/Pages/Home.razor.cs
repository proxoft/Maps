using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;
using Microsoft.AspNetCore.Components.Web;

namespace Proxoft.Maps.MapSamples.Client.Pages;

public partial class Home : IDisposable
{
    private IMap _map1 = NoMap.Instance;

    [Inject]
    public IMapFactory MapFactory { get; set; } = null!;

    ElementReference MapHost { get; set; }

    public string Provider => this.MapFactory.Name;

    private List<string> MapLog { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        LatLng center = new()
        {
            Latitude = -34.397m,
            Longitude = 150.644m
        };

        _map1 = await this.MapFactory.Initialize(
            new MapOptions
            {
                Center = center,
                Zoom = 10
            },
            this.MapHost
        );

        _map1.OnCenterChanged()
            .Subscribe(ll => this.AddMapLog($"center changed {ll.Latitude}, {ll.Longitude}"));
        _map1.OnClick()
            .Subscribe(ll => this.AddMapLog($"click: {ll.Latitude}, {ll.Longitude}"));
    }

    public void Dispose()
    {
        _map1.Dispose();
    }

    private void AddMapLog(string message)
    {
        this.MapLog.Add(message);
        this.StateHasChanged();
    }
}
