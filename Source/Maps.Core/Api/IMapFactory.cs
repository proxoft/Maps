using System.Threading.Tasks;

namespace Proxoft.Maps.Core.Api
{
    public interface IMapFactory
    {
        string Name { get; }

        Task<IMap> Initialize(string elementId, MapOptions options);
    }
}
