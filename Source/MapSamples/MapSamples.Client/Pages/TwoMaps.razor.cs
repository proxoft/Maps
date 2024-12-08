using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.MapSamples.Client.Pages;

public sealed partial class TwoMaps : IDisposable
{
    private IMap _map1 = NoMap.Instance;
    private IMap _map2 = NoMap.Instance;

    ElementReference Map1Host { get; set; }

    ElementReference Map2Host { get; set; }

    private List<string> Map1Log { get; set; } = new();

    private List<string> Map2Log { get; set; } = new();

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            LatLng center = new()
            {
                Latitude = -34.397m,
                Longitude = 150.644m
            };

            this.MapFactory.Initialize(
                new MapOptions
                {
                    Center = center,
                    Zoom = 10
                },
                this.Map1Host
                )
                .Subscribe(m =>
                {
                    _map1 = m;
                    _map1.OnCenterChanged()
                        .Subscribe(ll =>
                        {
                            this.Map1Log.Add("center changed");
                            this.StateHasChanged();
                        });

                    _map1.AddMarker(new MarkerOptions { Position = center, TraceJs = true });
                });

            this.MapFactory.Initialize(
                new MapOptions
                {
                    Center = new LatLng
                    {
                        Latitude = 19.397m,
                        Longitude = 87.644m
                    },
                    Zoom = 7
                },
                this.Map2Host
                )
                .Subscribe(m => {
                    _map2 = m;
                    _map2.OnCenterChanged()
                        .Subscribe(ll =>
                        {
                            this.Map2Log.Add("center changed");
                            this.StateHasChanged();
                        });
                });
        }
    }

    private void PanToClick()
    {
        _map1.PanTo(new LatLng { Latitude = 48.15m, Longitude = 17.6m });
    }

    public void Dispose()
    {
        _map1.Dispose();
        _map2.Dispose();
    }
}
