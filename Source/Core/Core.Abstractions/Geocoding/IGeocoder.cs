using System.Threading.Tasks;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.Geocoding;

public interface IGeocoder
{
    ApiStatus Status { get; }

    Task<Either<ErrorStatus, Address>> Geocode(string location);

    Task<Either<ErrorStatus, Address>> Geocode(string city, string? street = null, string? streetNumber = null, string? country = null);

    Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng);

    Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude);

    Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string location);

    Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string city, string streetName);
}
