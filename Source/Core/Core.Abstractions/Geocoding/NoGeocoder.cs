using System.Threading.Tasks;
using Proxoft.Maps.Core.Abstractions.Models;

namespace Proxoft.Maps.Core.Abstractions.Geocoding;

public class NoGeocoder : IGeocoder
{
    public static readonly NoGeocoder Instance = new();

    private NoGeocoder()
    {
    }

    public ApiStatus Status => ApiStatus.NotAvailable;

    public Task<Either<ErrorStatus, Address>> Geocode(string location)
        => Task.FromResult<Either<ErrorStatus, Address>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, Address>> Geocode(string city, string? street = null, string? streeNumber = null, string? country = null)
        => Task.FromResult<Either<ErrorStatus, Address>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng)
        => Task.FromResult<Either<ErrorStatus, Address>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude)
        => Task.FromResult<Either<ErrorStatus, Address>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string location) =>
        Task.FromResult<Either<ErrorStatus, StreetGeometry>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(string city, string streetName) =>
        Task.FromResult<Either<ErrorStatus, StreetGeometry>>(ErrorStatus.UnknownError);

    public Task<Either<ErrorStatus, StreetGeometry>> GeocodeStreet(decimal latitude, decimal longitude) =>
        Task.FromResult<Either<ErrorStatus, StreetGeometry>>(ErrorStatus.UnknownError);
}
