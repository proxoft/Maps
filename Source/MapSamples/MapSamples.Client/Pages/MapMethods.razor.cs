using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.MapSamples.Client.Pages;

public partial class MapMethods
{
    private IMap _map = NoMap.Instance;

    ElementReference MapHost { get; set; }

    private List<string> MapLog { get; set; } = [];

    protected override void OnAfterFirstRender()
    {
        base.OnAfterFirstRender();

        LatLng center = new()
        {
            Latitude = -34.397m,
            Longitude = 150.644m
        };

        this.MapFactory.Initialize(
            new MapOptions { Center = center, Zoom = 7, TraceJs = true },
            this.MapHost
        )
            .Take(1)
            .Do(map =>
            {
                _map = map;
                _draggable = _map.IsDraggable();
                _map.OnEvent
                    .Where(e => e is not MouseMoveEvent)
                    .Subscribe(e => this.AddLog(e.Name));
            })
            .Subscribe();
    }

    private bool _draggable;

    private decimal PanLat { get; set; } = 48.15m;
    private decimal PanLng { get; set; } = 17.6m;

    private decimal CenterLat { get; set; } = 48.15m;
    private decimal CenterLng { get; set; } = 17.6m;

    private decimal FitToBoundsLat { get; set; } = 48.15m;
    private decimal FitToBoundsLng { get; set; } = 17.6m;

    private int Zoom { get; set; } = 7;

    private LatLngBounds Bounds { get; set; } = LatLngBounds.Empty;

    private LatLng Center { get; set; } = LatLng.None;

    private bool Draggable
    {
        get => _map.IsDraggable();
        set => _map.SetDraggable(value);
    }

    private void SetCenterClick()
    {
        _map.SetCenter(new LatLng { Latitude = this.CenterLat, Longitude = this.CenterLng });
    }

    private void GetCenterClick()
    {
        System.Console.WriteLine("getting center");
        this.Center = _map.GetCenter();
        this.StateHasChanged();
    }

    private void PanToClick()
    {
        _map.PanTo(new LatLng { Latitude = this.PanLat, Longitude = this.PanLng });
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

    private void UpdateDraggable()
    {
        _map.SetDraggable(_draggable);
    }

    private void AddLog(string logMessage)
    {
        this.MapLog.Add(logMessage);
        this.StateHasChanged();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _map.Dispose();
        }
        base.Dispose(disposing);
    }
}
