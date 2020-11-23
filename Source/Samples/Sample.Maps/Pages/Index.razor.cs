using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Sample.Maps.Pages
{
    public partial class Index
    {
        private Proxoft.Maps.Core.Api.IMap _map1;
        private Proxoft.Maps.Core.Api.IMap _map2;

        [Inject]
        public Proxoft.Maps.Core.Api.IMapFactory MapFactory { get; set; }

        public string Provider => this.MapFactory.Name;

        private List<string> Map1Log { get; set; } = new();
        private List<string> Map2Log { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _map1 = await MapFactory.Initialize("map-container", new Proxoft.Maps.Core.Api.MapOptions
                {
                    Center = new Proxoft.Maps.Core.Api.LatLng
                    {
                        Latitude = -34.397m,
                        Longitude = 150.644m
                    },
                    Zoom = 10
                });

                _map1.OnCenter
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(ll =>
                    {
                        this.Map1Log.Add("center changed");
                        this.StateHasChanged();
                    });

                _map2 = await MapFactory.Initialize("map-container-2", new Proxoft.Maps.Core.Api.MapOptions
                {
                    Center = new Proxoft.Maps.Core.Api.LatLng
                    {
                        Latitude = 19.397m,
                        Longitude = 87.644m
                    },
                    Zoom = 7
                });

                _map2.OnCenter
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(ll =>
                    {
                        this.Map2Log.Add("center changed");
                        this.StateHasChanged();
                    });
            }
        }
    }
}
