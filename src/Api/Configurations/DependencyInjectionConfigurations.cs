using MediatR;
using S4Capital.Api.Core;
using S4Capital.Api.Infrastructure.Repositories;
using System.Reflection;

namespace S4Capital.Api.Api.Configurations;

public static class DependencyInjectionConfigurations
{
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<INotificationHandler<Notification>, NotificationHandler>();
        services.AddScoped<INotificationManager, NotificationManager>();

        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}
