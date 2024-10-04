using S4Capital.Api.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContextConfiguration(configuration);
builder.Services.AddJwtConfiguration(configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddDependencyInjectionConfiguration();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseApiConfiguration();

app.UpdateDatabase();


await app.RunAsync();

public partial class Program
{
    protected Program()
    { }
}
