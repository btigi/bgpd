using bgpd;
using DotNetify;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddDotNetify().AddSignalR();

var app = builder.Build();

app.MapHub<DotNetifyHub>("/dotnetify");

app.MapVM("GameInfo", (IGameService service) => new
{
    service.Entities
});

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();