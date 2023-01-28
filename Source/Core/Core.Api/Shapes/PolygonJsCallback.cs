using System;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes;

internal class PolygonJsCallback : ApiObjectJsCallback<PolygonJsCallback>
{
    public PolygonJsCallback(Action<Event> onEvent) : base(onEvent)
    {
    }
}
