using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Korrekturmanagementsystem.Endpoints
{
    public static class LogoutEndpoint
    {
        public static void MapLogoutEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/account/logout", async (HttpContext http) =>
            {
                await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Redirect("/account/login");
            });
        }
    }
}
