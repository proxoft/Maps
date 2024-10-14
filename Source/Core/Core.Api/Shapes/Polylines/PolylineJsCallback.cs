using System;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

internal sealed class PolylineJsCallback : ApiObjectJsCallback<PolylineJsCallback>
{
    public PolylineJsCallback(Action<Event> onEvent) : base(onEvent)
    {
    }
}
