using Microsoft.JSInterop;
using Proxoft.Maps.Core.Api.Maps;

namespace Proxoft.Maps.OpenStreetMap.Maps.Models.Maps;

internal class OsmMap(
    string mapId,
    OsmMapObjectsFactory mapObjectsFactory,
    IJSInProcessObjectReference jsModule) : Map(mapId, mapObjectsFactory, jsModule)
{
}
