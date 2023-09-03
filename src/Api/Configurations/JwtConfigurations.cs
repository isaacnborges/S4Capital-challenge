using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using S4Capital.Api.Api.ModelSettings;
using System.Text;

namespace S4Capital.Api.Api.Configurations;

public static class JwtConfigurations
{
    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection("JwtModelSettings");
        services.Configure<JwtModelSettings>(jwtSettingsSection);
        var jwtModelSettings = jwtSettingsSection.Get<JwtModelSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModelSettings.Secret)),
                ValidAudience = jwtModelSettings.Audience,
                ValidIssuer = jwtModelSettings.Issuer,
                ValidateIssuer = true,
                ValidateAudience = true
            };
        });
    }
}
