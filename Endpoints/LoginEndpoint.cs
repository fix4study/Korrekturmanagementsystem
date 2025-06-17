using System.Security.Claims;
using Korrekturmanagementsystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Korrekturmanagementsystem.Endpoints
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/account/login", async (
                HttpContext http,
                IUserService userService,
                IPasswordService passwordService,
                [FromForm] string username,
                [FromForm] string password
            ) =>
            {
                var user = await userService.GetUserByUsernameAsync(username);
                if (user is null || !passwordService.VerifyPassword(user.HashedPassword, password))
                {
                    return Results.Redirect("/account/login?error=true");
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, user.SystemRoleName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                        });

                return Results.Redirect("/");
            }).AllowAnonymous().DisableAntiforgery();
        }
    }
}
