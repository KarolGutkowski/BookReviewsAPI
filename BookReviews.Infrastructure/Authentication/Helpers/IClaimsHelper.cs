using BookReviews.Domain.Models.DTOs;
using System.Security.Claims;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IClaimsHelper
{
    ClaimsPrincipal GenerateUserClaimsPrincipal(UserDTO user, string claimsSchema);
}
