using System.Globalization;
using Proxoft.Maps.OpenStreetMap.Geocoding.Models;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Parsing
{
    internal static class ResultExtensions
    {
        public static Core.Geocoding.Address ToAddress(this Result result)
        {
            return new Core.Geocoding.Address
            {
                Country = result.address.ToCountry(),
                City = result.address.ToCity(),
                Street = result.address.road,
                StreetNumber = result.address.ToStreetNumber(),
                Zip = result.address.postcode,
                LatLng = result.ToLatLng()
            };
        }

        private static Core.Api.LatLng ToLatLng(this Result result)
        {
            if(!decimal.TryParse(result.lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat)
                || !decimal.TryParse(result.lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng))
            {
                return Core.Api.LatLng.None;
            }

            return new Core.Api.LatLng
            {
                Latitude = lat,
                Longitude = lng
            };
        }

        private static string ToCountry(this Address address)
        {
            return address.country ?? address.state;
        }

        private static string ToCity(this Address address)
        {
            return address.village
                ?? address.town
                ?? address.city_district
                ?? address.municipality;
        }

        private static string ToStreetNumber(this Address address)
        {
            return address.house_number
                ?? address.house_name;
        }
    }
}
