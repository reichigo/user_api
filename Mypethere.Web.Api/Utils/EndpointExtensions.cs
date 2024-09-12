using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Mypethere.Web.Api.Utils;

public static class EndpointExtensions
{
    public static IServiceCollection AddAppEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var descriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(descriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app
            .Services
            .GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
