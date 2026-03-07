using EC.Spotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSpotify(builder.Configuration.GetSection("Spotify"));

// using direct options
//builder.Services.AddSpotify((o) => {
//    o.ClientId = "clientId";
//    o.ClientSecret = "clientSecret";
//    o.RedirectUri = "";
//    o.Scopes = [
//      "user-read-playback-state",
//      "user-modify-playback-state",
//      "user-library-read"
//    ];
//});

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
