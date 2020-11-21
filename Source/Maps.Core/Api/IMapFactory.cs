using System.Threading.Tasks;

namespace Proxoft.Maps.Core.Api
{
    public interface IMapFactory
    {
        Task<IMap> Initialize(string cssSelector);
    }
}
