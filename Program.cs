using Korrekturmanagementsystem.Components;
using Korrekturmanagementsystem.Endpoints;
using Korrekturmanagementsystem.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Korrekturmanagementsystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpContextAccessor();

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

        app.Run();
    }
}
