using System;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Markers
{
    internal class Hooks
    {
        public Action<string> OnRemove { get; set; } = _ => { };
    }
}
