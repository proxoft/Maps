using System;
using Microsoft.Extensions.DependencyInjection;
using Proxoft.Maps.Core.Api.Factories;
using Proxoft.Maps.OpenStreetMap.Maps;

namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders;

internal abstract class MapBuilderBase : IMapBuilder
{
    public abstract IMapBuilder UseIdFactory<TIdFactory>() where TIdFactory : IIdFactory;

    internal abstract void Register(IServiceCollection services, ServiceLifetime serviceLifetime);
}

internal sealed class MapBuilder : MapBuilderBase
{
    private Type _idFactory = typeof(IdFactory);

    public override IMapBuilder UseIdFactory<TIdFactory>()
    {
        _idFactory = typeof(TIdFactory);
        return this;
    }

    internal override void Register(IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        services.Add(new ServiceDescriptor(typeof(IIdFactory), _idFactory, serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IMapFactory), typeof(MapFactory), serviceLifetime));
    }
}

internal sealed class NoMapBuilder : MapBuilderBase
{
    public static readonly NoMapBuilder Instance = new();

    private NoMapBuilder()
    {
    }

    public override IMapBuilder UseIdFactory<TIdFactory>()
    {
        return this;
    }

    internal override void Register(IServiceCollection services, ServiceLifetime serviceLifetime)
    {
    }
}
