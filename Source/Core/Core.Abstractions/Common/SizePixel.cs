﻿namespace Proxoft.Maps.Core.Abstractions.Common;

public record SizePixel
{
    public uint Width { get; set; }

    public uint Height { get; set; }

    public bool IsZero => this.Width == 0 && this.Height == 0;
}
