using Proxoft.Maps.Core.Api.Icons;

namespace Proxoft.Maps.Core.Api;

public class MarkerOptions
{
    public LatLng Position { get; set; }

    public bool Draggable { get; set; } = true;

    public Opacity Opacity { get; set; } = Opacity.Visible;

    public IconOptions Icon { get; set; } = new ImageIcon();

    /// <summary>
    /// Logs JS activity in console
    /// </summary>
    public bool TraceJs { get; set; }
}
