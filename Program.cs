using Korrekturmanagementsystem;
using Korrekturmanagementsystem.Components;
using Korrekturmanagementsystem.Endpoints;
using Korrekturmanagementsystem.Extensions;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});

var config = builder.Configuration;
builder.Services
    .AddApplicationServices(config)
    .AddRepositories(config)
    .AddDatabase(config)
    .AddAuthenticationSetup()
    .AddHttpClients(config);

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapLoginEndpoint();
app.MapLogoutEndpoint();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().DisableAntiforgery();

await app.RunAsync();
