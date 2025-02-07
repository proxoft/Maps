using System;
using Proxoft.Maps.Core.Api.Core;

namespace Proxoft.Maps.Core.Api.Shapes.Rectangles;

internal sealed class RectangleJsCallback(Action<Event> onEvent) : ApiObjectJsCallback<RectangleJsCallback>(onEvent)
{
}
