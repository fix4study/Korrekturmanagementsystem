using Korrekturmanagementsystem.Components;
using Korrekturmanagementsystem.Endpoints;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Korrekturmanagementsystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient("BackendAPI", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7134");
        });

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("BackendAPI"));

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/access-denied";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        builder.Services.AddAuthorization();

        var config = builder.Configuration;
        builder.Services
            .AddApplicationServices(config)
            .AddRepositories(config)
            .AddDatabase(config);

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
