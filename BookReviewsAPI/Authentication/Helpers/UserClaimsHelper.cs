using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Authentication.Schemas;
using BookReviewsAPI.Models;
using System.Security.Claims;

namespace BookReviewsAPI.Authentication.Helpers
{
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
}