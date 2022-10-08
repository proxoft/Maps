using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.Sample.Maps.Pages
{
    public sealed partial class TwoMaps : IDisposable
    {
        private IMap _map1;
        private IMap _map2;

        [Inject]
        public IMapFactory MapFactory { get; set; }

        ElementReference Map1Host { get; set; }

        ElementReference Map2Host { get; set; }

        public string Provider => this.MapFactory.Name;

        private List<string> Map1Log { get; set; } = new();
        private List<string> Map2Log { get; set; } = new();

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

                _map1 = await MapFactory.Initialize(new MapOptions
                {
                    Center = center,
                    Zoom = 10
                },
                this.Map1Host);

                _map1.OnCenter()
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(ll =>
                    {
                        this.Map1Log.Add("center changed");
                        this.StateHasChanged();
                    });

                _map1.AddMarker(new MarkerOptions { Position = center, TraceJs = true });

                _map2 = await MapFactory.Initialize(new MapOptions
                {
                    Center = new LatLng
                    {
                        Latitude = 19.397m,
                        Longitude = 87.644m
                    },
                    Zoom = 7
                },
                this.Map2Host);

                _map2.OnCenter()
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(ll =>
                    {
                        this.Map2Log.Add("center changed");
                        this.StateHasChanged();
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
}
