﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Sample.Maps.Pages
{
    public sealed partial class Index : IDisposable
    {
        private IMap _map1;

        [Inject]
        public IMapFactory MapFactory { get; set; }

        ElementReference MapHost { get; set; }

        public string Provider => this.MapFactory.Name;

        private List<string> MapLog { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                LatLng center = new ()
                {
                    Latitude = -34.397m,
                    Longitude = 150.644m
                };

                _map1 = await MapFactory.Initialize(new MapOptions
                {
                    Center = center,
                    Zoom = 10
                },
                this.MapHost);

                _map1.OnCenter()
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(ll =>
                    {
                        this.MapLog.Add("center changed");
                        this.StateHasChanged();
                    });
            }
        }

        public void Dispose()
        {
            _map1.Dispose();
        }
    }
}
