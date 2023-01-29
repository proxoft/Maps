using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class OsmMarker : MarkerBase, IMarker
    {
        private readonly Hooks _hooks;

        public OsmMarker(
            string markerId,
            IJSInProcessObjectReference jsModule,
            Hooks hooks) : base(markerId, jsModule)
        {
            _hooks = hooks ?? new Hooks();
        }

        protected override void ExecuteRemove()
        {
            _hooks?.OnRemove(this.Id);
        }
    }
}
