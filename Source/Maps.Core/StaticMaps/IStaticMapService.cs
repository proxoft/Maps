using System.Threading.Tasks;

namespace Proxoft.Maps.Core.StaticMaps
{
    public interface IStaticMapService
    {
        Task<byte[]> CreateImage(MapOptions options);
    }
}
