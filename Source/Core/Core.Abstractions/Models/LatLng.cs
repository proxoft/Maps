namespace Proxoft.Maps.Core.Abstractions.Models;

public record LatLng
{
    public static readonly LatLng None = new ();

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public static LatLng From(decimal lat, decimal lng) =>
        new()
        {
            Latitude = lat,
            Longitude = lng,
        };
}
