using BookReviews.Domain.Models.DataModels;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Infrastructure.Authentication.Schemas;
using System.Security.Claims;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public class UserClaimsHelper : IClaimsHelper
{
    public ClaimsPrincipal GenerateUserClaimsPrincipal(User user, string claimsSchema = AuthenticationSchemasConsts.DefaultSchema)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(AuthenticationPoliciesConsts.DefaultUserAuth, user.UserName));
        var indentity = new ClaimsIdentity(claims, claimsSchema);
        return new ClaimsPrincipal(indentity);
    }
}
