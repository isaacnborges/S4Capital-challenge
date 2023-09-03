using S4Capital.Api.Api.Configurations;

namespace S4Capital.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextConfiguration(Configuration);

        services.AddJwtConfiguration(Configuration);

        services.AddApiConfiguration();

        services.AddDependencyInjectionConfiguration();

        services.AddSwaggerConfiguration();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwaggerConfiguration();

        app.UseApiConfiguration();

        app.UpdateDatabase();
    }
}
