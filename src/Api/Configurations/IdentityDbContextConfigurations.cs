using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using S4Capital.Api.Infrastructure;

namespace S4Capital.Api.Api.Configurations;

public static class IdentityDbContextConfigurations
{
    public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
                providerOptions => providerOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null)));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<BlogDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
    {
        var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<BlogDbContext>();
        context.Database.Migrate();

        return app;
    }
}
