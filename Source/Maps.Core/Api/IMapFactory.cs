using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Proxoft.Maps.Core.Api
{
    public interface IMapFactory
    {
        string Name { get; }

        Task<IMap> Initialize(MapOptions options, ElementReference hostElement);
    }
}
