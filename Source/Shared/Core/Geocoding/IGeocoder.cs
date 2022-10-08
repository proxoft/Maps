using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Core.Geocoding
{
    public interface IGeocoder
    {
        ApiStatus Status { get; }

        Task<Either<ErrorStatus, Address>> Geocode(string location);

        Task<Either<ErrorStatus, Address>> Geocode(string city, string street = null, string streetNumber = null, string country = null);

        Task<Either<ErrorStatus, Address>> Geocode(LatLng latLng);

        Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude);
    }
}
