using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World! Identity");
app.MapGet("/protected", () => "Secret!").RequireAuthorization();
app.MapGet("/login", (HttpContext ctx) =>
{
    ctx.SignInAsync(new ClaimsPrincipal(new[]
    {
        new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        },
        CookieAuthenticationDefaults.AuthenticationScheme)
    }));

    return "ok";
});

app.Run();