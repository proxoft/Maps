using System.Threading.Tasks;
using Proxoft.Extensions.Options;
using Proxoft.Maps.Core.Api;

namespace Proxoft.Maps.Core.Geocoding
{
    public interface IGeocoder
    {
        Task<Either<ErrorStatus, Address>> Geocode(string location);

        Task<Either<ErrorStatus, LatLng>> Geocode(Address address);

        Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude);
    }
}
