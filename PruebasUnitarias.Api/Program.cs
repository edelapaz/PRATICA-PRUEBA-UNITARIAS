using PruebasUnitarias.Api.Application;
using PruebasUnitarias.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IUserService, InMemoryUserService>();

var app = builder.Build();

app.MapControllers();

await app.RunAsync();

public partial class Program
{
	protected Program()
	{
	}
}
