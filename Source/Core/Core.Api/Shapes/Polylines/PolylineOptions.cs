namespace Proxoft.Maps.Core.Api.Shapes.Polylines;

public record class PolylineOptions
{
    public LatLng[][] Lines { get; init; } = [];

    public Style Style { get; init; } = new Style();

    public bool TraceJs { get; init; }

    public static PolylineOptions SingleLine(LatLng[] latLngs, Style style, bool traceJs = false)
    {
        return new PolylineOptions()
        {
            Lines = [latLngs],
            Style = style,
            TraceJs = traceJs
        };
    }

    public static PolylineOptions MultiLine(LatLng[][] latLngs, Style style, bool traceJs = false)
    {
        return new PolylineOptions()
        {
            Lines = latLngs,
            Style = style,
            TraceJs = traceJs
        };
    }
}
