using Microsoft.AspNetCore.Authentication.Cookies;

namespace BookReviews.Infrastructure.Authentication.Schemas;

public static class AuthenticationSchemasConsts
{
    public const string DefaultSchema = CookieAuthenticationDefaults.AuthenticationScheme;
}
