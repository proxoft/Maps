using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Proxoft.Maps.Core.Api.Factories;

public interface IMapFactory
{
    string Name { get; }

    IObservable<IMap> Initialize(MapOptions options, ElementReference hostElement);
}
