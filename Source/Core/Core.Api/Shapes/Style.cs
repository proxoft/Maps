namespace Proxoft.Maps.Core.Api.Shapes;

public class Style
{
    public bool Stroke { get; set; } = true;

    public string Color { get; set; } = "#3388ff";

    public decimal Weight { get; set; } = 3;

    public decimal Opacity { get; set; } = 1;

    public string LineCap { get; set; } = "round";

    public string LineJoin { get; set; } = "round";

    public string? DashArray { get; set; }

    public string? DashOffset { get; set; }

    public bool Fill { get; set; } = true;

    public string FillColor { get; set; } = "#3388ff";

    public decimal FillOpacity { get; set; } = 0.2m;

    public string FillRule { get; set; } = "evenodd";

    public string? ClassName { get; set; }
}
