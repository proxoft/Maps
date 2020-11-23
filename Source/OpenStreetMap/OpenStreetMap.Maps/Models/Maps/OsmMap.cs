using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps
{
    internal class OsmMap : MapBase<OsmMap>
    {
        private readonly string _elementId;

        public OsmMap(string elementId, IJSInProcessObjectReference jsModule) : base(jsModule)
        {
            _elementId = elementId;
        }

        public void Initialize(MapOptions options) 
        {
            this.InvokeVoidJs("InitializeMap", new object[] { _elementId, options, this.SelfRef });
        }

        public static OsmMap Create(string elementId, MapOptions options, IJSInProcessObjectReference jsModule)
        {
            var map = new OsmMap(elementId, jsModule);
            map.Initialize(options);
            return map;
        }
    }
}
