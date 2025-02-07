using System;
using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Shapes.Rectangles;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Shapes;

internal class OsmRectangle(string id, Action<string> onRemove, IJSInProcessObjectReference jsModule) : Rectangle(id, onRemove, jsModule)
{
}
