using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Google.Maps.Models.Maps
{
    internal class GoogleMap: IMap
    {
        private readonly string _elementId;
        private readonly IJSObjectReference _jsRuntime;
        private DotNetObjectReference<GoogleMap> _ref;

        private GoogleMap(string elementId, IJSObjectReference jsRuntime)
        {
            _elementId = elementId;
            _jsRuntime = jsRuntime;
            _ref = DotNetObjectReference.Create(this);
        }

        private async Task Initialize(MapOptions options)
        {
            await _jsRuntime.InvokeVoidAsync("InitializeMap", new object[] { _elementId, options, _ref });
        }

        public static async Task<GoogleMap> Create(string elementId, MapOptions options, IJSObjectReference jsRuntime)
        {
            var map = new GoogleMap(elementId, jsRuntime);
            await map.Initialize(options);
            return map;
        }
    }
}
