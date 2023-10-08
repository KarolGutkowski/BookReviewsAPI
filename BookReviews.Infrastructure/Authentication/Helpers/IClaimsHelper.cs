using BookReviews.Domain.Models.DataModels;
using System.Security.Claims;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IClaimsHelper
{
    ClaimsPrincipal GenerateUserClaimsPrincipal(User user, string claimsSchema);
}
