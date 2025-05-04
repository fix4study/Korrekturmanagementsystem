using Microsoft.AspNetCore.Authentication.Cookies;

namespace Korrekturmanagementsystem.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthenticationSetup(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/access-denied";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient("BackendAPI", client =>
        {
            client.BaseAddress = new Uri(config["BackendApi:BaseUrl"] ?? "https://localhost:7134");
        });

        services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("BackendAPI"));

        return services;
    }
}
