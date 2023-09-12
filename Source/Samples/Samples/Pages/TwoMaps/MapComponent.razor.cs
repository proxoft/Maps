using Microsoft.AspNetCore.Components;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Api.Factories;

namespace Proxoft.Maps.Samples.Pages.TwoMaps;

public partial class MapComponent
{
    private IMap _map = NoMap.Instance;

    [Inject]
    public IMapFactory MapFactory { get; set; } = null!;

    private ElementReference Map1Host { get; set; }

    public string Provider => this.MapFactory.Name;

    private List<string> MapLog { get; set; } = new();

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);


    }
}
