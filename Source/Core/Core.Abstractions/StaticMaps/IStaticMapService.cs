using System.Threading.Tasks;

namespace Proxoft.Maps.Core.Abstractions.StaticMaps;

public interface IStaticMapService
{
    Task<Either<string, byte[]>> CreateImage(MapOptions options);
}
