using System.Globalization;
using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.Core.Abstractions.Models;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;

internal static class ResultExtensions
{
    public static Address ToAddress(this GeocodeResult result) =>
        new()
        {
            Country = result.address.ToCountry(),
            City = result.address.ToCity(),
            Street = result.address.road ?? "",
            StreetNumber = result.address.ToStreetNumber(),
            ConscriptionNumber = result.address.ToConscriptionNumber(),
            Zip = result.address.postcode ?? "",
            LatLng = result.ToLatLng()
        };

    private static LatLng ToLatLng(this GeocodeResult result)
    {
        if(!decimal.TryParse(result.lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat)
            || !decimal.TryParse(result.lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng))
        {
            return LatLng.None;
        }

        return new LatLng
        {
            Latitude = lat,
            Longitude = lng
        };
    }

    private static string ToCountry(this AddressDetail address)
    {
        return address.country ?? address.state ?? "";
    }

    private static string ToCity(this AddressDetail address)
    {
        return address.village
            ?? address.town
            ?? address.city
            ?? address.city_district
            ?? address.municipality
            ?? "";
    }

    private static string ToConscriptionNumber(this AddressDetail address)
    {
        string text = address.house_number
            ?? address.house_name
            ?? "";

        return text.Split('/', 2) switch
        {
            [string conscriptionNumber, _] => conscriptionNumber,
            _ => text
        };
    }

    private static string ToStreetNumber(this AddressDetail address)
    {
        string text = address.house_number
            ?? address.house_name
            ?? "";

        return text.Split('/', 2) switch
        {
            [string _, string number] => number,
            _ => text
        };
    }
}
