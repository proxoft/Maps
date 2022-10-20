using System.Drawing;
using Proxoft.Maps.Core.Abstractions.Common;
using Proxoft.Maps.Core.Abstractions.StaticMaps;

namespace Proxoft.Maps.Core.Api.Icons;

public class ImageIcon : IconOptions
{
    public ImageIcon()
    {
        this.Size = new(25, 41);
        this.IconAnchor = new(12, 41);
        this.PopupAnchor = new(1, -34);

    }
    public string Url { get; set; } = "";

    public string RetinaUrl { get; set; } = "";

    public string ShadowUrl { get; set; } = "";

    public Point ShadowAnchor { get; set; } = new(12, 41);

    public Size ShadowSize { get; set; } = new Size(41, 41);
}
