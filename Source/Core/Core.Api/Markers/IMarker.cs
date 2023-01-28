using System;

namespace Proxoft.Maps.Core.Api;

public interface IMarker : IApiObject
{
    void SetPosition(decimal latitude, decimal longitude);

    void SetPosition(LatLng latLng);

    void SetDraggable(bool draggable);

    void SetOpacity(Opacity opacity);

    void SetIcon(IconOptions icon);
}
