using App.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoadApplicationSettings();
builder.AddSecretManager();
await builder.AddHttpClientAuthenticationAsync();
builder.AddApiSecuret();
builder.AddConnections();
builder.AddTelemetric();

var app = builder.Build();

app.UseConfigAuthentication();
app.UseConfigRoutes();

app.Run();