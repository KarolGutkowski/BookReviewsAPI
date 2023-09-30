using BookReviewsAPI.Models;
using System.Security.Claims;

namespace BookReviewsAPI.Authentication.Helpers
{
    public interface IClaimsHelper
    {
        ClaimsPrincipal GenerateUserClaimsPrincipal(User user, string claimsSchema);
    }
}