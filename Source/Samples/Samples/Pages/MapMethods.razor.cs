﻿using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Samples.Pages;

public partial class MapMethods : IDisposable
{
    private IMap _map = NoMap.Instance;

    [Inject]
    public IMapFactory MapFactory { get; set; } = null!;

    ElementReference MapHost { get; set; }

    private List<string> MapLog { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            LatLng center = new()
            {
                Latitude = -34.397m,
                Longitude = 150.644m
            };

            _map = await this.MapFactory.Initialize(new MapOptions { Center = center, Zoom = 7, TraceJs = true }, this.MapHost);
            _map.OnEvent
                .Where(e => !(e is MouseMoveEvent))
                .Subscribe(e => this.AddLog(e.Name));

        }
    }

    private decimal PanLat { get; set; } = 48.15m;
    private decimal PanLng { get; set; } = 17.6m;

    private decimal CenterLat { get; set; } = 48.15m;
    private decimal CenterLng { get; set; } = 17.6m;

    private decimal FitToBoundsLat { get; set; } = 48.15m;
    private decimal FitToBoundsLng { get; set; } = 17.6m;

    private int Zoom { get; set; } = 7;

    private LatLngBounds Bounds { get; set; } = LatLngBounds.Empty;
    private LatLng Center { get; set; } = LatLng.None;

    private void SetCenterClick()
    {
        _map.SetCenter(new LatLng { Latitude = CenterLat, Longitude = CenterLng });
    }

    private void GetCenterClick()
    {
        System.Console.WriteLine("getting center");
        this.Center = _map.GetCenter();
        this.StateHasChanged();
    }

    private void PanToClick()
    {
        _map.PanTo(new LatLng { Latitude = PanLat, Longitude = PanLng });
    }

    private void ZoomToClick()
    {
        _map.ZoomTo(new ZoomLevel(this.Zoom));
    }

    private void GetBoundsClick()
    {
        this.Bounds = _map.GetBounds();
        this.StateHasChanged();
    }

    private void FitToBoundsClick()
    {
        LatLngBounds bounds = LatLngBounds.FromPosition(new LatLng
        {
            Latitude = this.FitToBoundsLat,
            Longitude = this.FitToBoundsLng
        });

        _map.FitBounds(bounds);
    }

    private void AddLog(string logMessage)
    {
        this.MapLog.Add(logMessage);
        this.StateHasChanged();
    }

    public void Dispose()
    {
        _map.Dispose();
    }
}
