﻿namespace Proxoft.Maps.Core.Api;

public abstract class Event
{
    protected Event()
    {
    }

    public string Name => this.GetType().Name;

    public string SourceId { get; internal set; } = "";
}
