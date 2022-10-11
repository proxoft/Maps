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

        public override void Remove()
        {
            base.Remove();

            if (this.IsRemoved)
            {
                return;
            }

            _hooks?.OnRemove(this.MarkerId);
        }
    }
}
