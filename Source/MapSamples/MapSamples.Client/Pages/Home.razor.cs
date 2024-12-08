using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.MapSamples.Client.Pages;

public sealed partial class Home
{
    private IMap _map1 = NoMap.Instance;

    ElementReference MapHost { get; set; }

    public string Provider => this.MapFactory.Name;

    private List<string> MapLog { get; set; } = [];

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
                Zoom = 10,
                TraceJs = true,
            },
            this.MapHost
        );

        _map1.OnCenterChanged()
            .Subscribe(ll => this.AddMapLog($"center changed {ll.Latitude}, {ll.Longitude}"));
        _map1.OnClick()
            .Subscribe(ll => this.AddMapLog($"click: {ll.Latitude}, {ll.Longitude}"));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _map1.Dispose();
        }

        base.Dispose(disposing);
    }

    private void AddMapLog(string message)
    {
        this.MapLog.Add(message);
        this.StateHasChanged();
    }
}
