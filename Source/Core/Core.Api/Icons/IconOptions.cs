using System.Drawing;

namespace Proxoft.Maps.Core.Api;

public abstract class IconOptions
{
    public string Discriminator => this.GetType().Name;

    public Size Size { get; set; }

    public Point IconAnchor { get; set; }

    public Point PopupAnchor { get; set; }

    public string ClassName { get; set; }
}
