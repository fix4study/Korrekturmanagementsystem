using Korrekturmanagementsystem;
using Korrekturmanagementsystem.Components;
using Korrekturmanagementsystem.Endpoints;
using Korrekturmanagementsystem.Extensions;
using Korrekturmanagementsystem.UnitOfWork;
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
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddApplicationServices()
    .AddRepositories()
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

app.MapLogoutEndpoint();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
