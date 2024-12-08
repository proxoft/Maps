using System;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes.Polygones;

internal class PolygonJsCallback(Action<Event> onEvent) : ApiObjectJsCallback<PolygonJsCallback>(onEvent)
{
}
