using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Api;
using Proxoft.Maps.Core.Geocoding;
using Proxoft.Maps.Google.Common;

using static Proxoft.Maps.Google.Geocoding.AddressComponentTypes;

namespace Proxoft.Maps.Google.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private readonly HttpClient _http = new ();
        private readonly string _urlBase;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ApiStatus Status => ApiStatus.Available;

        public GoogleGeocoder(GoogleApiConfiguration configuration)
        {
            _urlBase = $"https://maps.googleapis.com/maps/api/geocode/json?&key={configuration.ApiKey}&language={configuration.Language}&region={configuration.Region}";
        }

        public Task<Either<ErrorStatus, Address>> Geocode(string location)
            => this.ExecuteGeocode($"address={location}");


        public Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
            => this.ExecuteGeocode($"latlng={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");

        private async Task<Either<ErrorStatus, Address>> ExecuteGeocode(string geocodingParameter)
        {
            var requestUrl = _urlBase + "&" + geocodingParameter;

            using var response = await _http.GetAsync(requestUrl);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return  ErrorStatus.UnknownError;
            }

            var contentString = await response.Content.ReadAsStringAsync();
            var either = this.Parse(contentString);

            return either;
        }

        private Either<ErrorStatus, Address> Parse(string jsonResult)
        {
            var geocodeResponse = JsonSerializer.Deserialize<GeocoderResponse>(jsonResult, options: _options);

            return geocodeResponse.status switch
            {
                "OK" => ParseResult(geocodeResponse.results.First()),
                "ZERO_RESULTS" => ErrorStatus.ZeroResults,
                "OVER_QUERY_LIMIT" => ErrorStatus.QuotaError,
                "REQUEST_DENIED" or "INVALID_REQUEST" => ErrorStatus.InvalidRequest,
                "UNKNOWN_ERROR" or "ERROR" => ErrorStatus.ServerError,
                _ => ErrorStatus.UnknownError,
            };
        }

        private static Address ParseResult(GeocoderResult result)
        {
            var registerNumber = result.GetLongNameOfType(Premise);
            var streetNumber = result.GetLongNameOfType(StreetNumber);
            var street = result.GetLongNameOfType(Route);
            var country = result.GetLongNameOfType(Country);
            var city = result.GetLongNameOfType(Locality, Political, Sublocality);
            var zip = result.GetLongNameOfType(PostalCode);

            var latitude = Convert.ToDecimal(result.geometry.location.Lat, CultureInfo.InvariantCulture);
            var longitude = Convert.ToDecimal(result.geometry.location.Lng, CultureInfo.InvariantCulture);

            return new Address
            {
                Location = result.formatted_address,
                Country = country,
                City = city,
                Street = street,
                RegisterNumber = registerNumber,
                StreetNumber = streetNumber,
                Zip = zip,
                LatLng = new Core.LatLng
                {
                    Latitude = latitude,
                    Longitude = longitude
                }
            };
        }

        public Task<Either<ErrorStatus, Address>> Geocode(Address address)
        {
            throw new NotImplementedException();
        }

        public Task<Either<ErrorStatus, Address>> Geocode(Core.LatLng latLng)
        {
            throw new NotImplementedException();
        }

        public Task<Either<ErrorStatus, Address>> Geocode(string city, string street = null, string streetNumber = null, string country = null)
        {
            throw new NotImplementedException();
        }
    }
}
