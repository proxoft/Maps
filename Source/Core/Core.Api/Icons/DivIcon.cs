using System.Drawing;
using Proxoft.Maps.Core.Abstractions.Common;

namespace Proxoft.Maps.Core.Api.Icons;

public class HtmlIcon : IconOptions
{
    public HtmlIcon()
    {
        this.Size = new(20, 20);
        this.IconAnchor = new Point(10, 10);
        this.PopupAnchor = new(1, -34);
    }

    public string Html { get; set; }
}