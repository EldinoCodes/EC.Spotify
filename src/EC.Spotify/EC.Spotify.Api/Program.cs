using EC.Spotify.Api;
using EC.Spotify.Api.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IHttpProvider, HttpProvider>();
builder.Services.AddScoped<EC.Spotify.Api.Services.IAuthorizationService, EC.Spotify.Api.Services.AuthorizationService>();
builder.Services.AddScoped<EC.Spotify.Api.Services.ISpotifyService, EC.Spotify.Api.Services.SpotifyService>();
builder.Services.AddScoped<EC.Spotify.Api.Services.ITokenService, EC.Spotify.Api.Services.TokenService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
