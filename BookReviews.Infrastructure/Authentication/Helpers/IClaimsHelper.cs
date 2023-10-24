using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IClaimsHelper
{
    ClaimsPrincipal GenerateUserClaimsPrincipal(UserDTO user, string claimsSchema);
    User? TryToGetUserAccountDetails(HttpContext httpContext);
}
