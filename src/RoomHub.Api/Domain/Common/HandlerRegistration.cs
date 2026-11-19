using System.Reflection;

namespace RoomHub.Api.Domain.Common;

public static class HandlerRegistration
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterface = typeof(ICommandHandler<,>);

        foreach (var implementation in assembly.GetTypes()
                     .Where(type => type is { IsClass: true, IsAbstract: false }))
        {
            foreach (var contract in implementation.GetInterfaces()
                         .Where(type => type.IsGenericType &&
                                        type.GetGenericTypeDefinition() == handlerInterface))
            {
                services.AddScoped(contract, implementation);
            }
        }

        return services;
    }
}
