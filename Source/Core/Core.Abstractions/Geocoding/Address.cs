using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.Geocoding;

public record Address
{
    public string Location { get; init; } = "";

    public string Country { get; init; } = "";

    public string City { get; init; } = "";

    public string Street { get; init; } = "";

    public string ConscriptionNumber { get; init; } = "";

    public string StreetNumber { get; init; } = "";

    public string Zip { get; init; } = "";

    public LatLng LatLng { get; init; } = LatLng.None;
}
