using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Api.Factories;

namespace Proxoft.Maps.MapSamples.Client.Pages;

public class MapComponent : ComponentBase, IDisposable
{
    [Inject]
    public IMapFactory MapFactory { get; set; } = null!;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            this.OnAfterFirstRender();
        }
    }

    protected virtual void OnAfterFirstRender()
    {
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
