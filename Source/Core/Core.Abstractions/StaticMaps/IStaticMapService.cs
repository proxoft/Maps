using System.Threading.Tasks;
using Proxoft.Extensions.Options;

namespace Proxoft.Maps.Core.Abstractions.StaticMaps;

public interface IStaticMapService
{
    Task<Either<string, byte[]>> CreateImage(MapOptions options);
}
