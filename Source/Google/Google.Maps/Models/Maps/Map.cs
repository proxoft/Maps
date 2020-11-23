﻿using System;
using System.Reactive.Subjects;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Google.Maps.Models.Maps
{
    internal class GoogleMap: IMap
    {
        private readonly string _elementId;
        private readonly IJSInProcessObjectReference _jsRuntime;
        private readonly DotNetObjectReference<GoogleMap> _ref;

        private readonly Subject<LatLng> _onCenter = new ();

        public IObservable<LatLng> OnCenter => _onCenter;

        public IObservable<int> OnZoom => throw new NotImplementedException();

        [JSInvokable]
        public void OnCenterChanged(LatLng latLng)
            => _onCenter.OnNext(latLng);

        private GoogleMap(string elementId, IJSInProcessObjectReference jsRuntime)
        {
            _elementId = elementId;
            _jsRuntime = jsRuntime;
            _ref = DotNetObjectReference.Create(this);
        }

        private void Initialize(MapOptions options)
        {
            _jsRuntime.InvokeVoid("InitializeMap", new object[] { _elementId, options, _ref });
        }

        public static GoogleMap Create(string elementId, MapOptions options, IJSInProcessObjectReference jsRuntime)
        {
            var map = new GoogleMap(elementId, jsRuntime);
            map.Initialize(options);
            return map;
        }

        public void Dispose()
        {
            _onCenter.Dispose();
        }

        public void PanTo(LatLng center)
        {
            throw new NotImplementedException();
        }

        public IMarker AddMarker(MarkerOptions options)
        {
            throw new NotImplementedException();
        }

        public void ZoomTo(int zoom)
        {
            throw new NotImplementedException();
        }
    }
}
