using System.Threading.Tasks;
using Proxoft.Extensions.Options;

namespace Proxoft.Maps.Core.Geocoding
{
    public interface IGeocoder
    {
        Task<Either<ErrorStatus, Address>> Geocode(string location);

        Task<Either<ErrorStatus, Address>> Geocode(decimal latitude, decimal longitude);
    }
}
