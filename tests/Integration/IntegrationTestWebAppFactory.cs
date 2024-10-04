using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using S4Capital.Api.Infrastructure;
using S4Capital.Tests.Support;
using System.Security.Claims;
using Testcontainers.MsSql;

namespace S4Capital.Tests.Integration;
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<BlogDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            var defaultClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)
            };

            services.AddSingleton<IPolicyEvaluator>(x => new PolicyEvaluatorHelper(defaultClaims));
            services.AddMvcCore(options => options.Filters.Add(new UserFilterHelper(defaultClaims)));
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}