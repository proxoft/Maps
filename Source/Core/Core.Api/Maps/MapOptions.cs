﻿namespace Proxoft.Maps.Core.Api;

public record MapOptions
{
    public LatLng Center { get; set; } = new LatLng();

    public int Zoom { get; set; }

    public bool Draggable { get; set; } = true;

    /// <summary>
    /// Logs JS activity in console
    /// </summary>
    public bool TraceJs { get; set; }
}
