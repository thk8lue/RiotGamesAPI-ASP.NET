using RiotAPI.Data;
using RiotAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// DI µî·Ï
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
