using System;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes.Circles;

internal sealed class CircleJsCallback(Action<Event> onEvent) : ApiObjectJsCallback<CircleJsCallback>(onEvent)
{
}
