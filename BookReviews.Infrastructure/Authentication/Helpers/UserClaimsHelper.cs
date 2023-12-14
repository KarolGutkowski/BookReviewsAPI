using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Infrastructure.Authentication.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public class UserClaimsHelper : IClaimsHelper
{
    private readonly ILogger<UserClaimsHelper> _logger;
    private readonly IUsersRepository _usersRepository;
    public UserClaimsHelper(ILogger<UserClaimsHelper> logger,
        IUsersRepository usersRepository)
    {
        _logger = logger;
        _usersRepository = usersRepository;
    }
    public ClaimsPrincipal GenerateUserClaimsPrincipal(UserLoginDTO user, string claimsSchema = AuthenticationSchemasConsts.DefaultSchema)
    {
        var claims = new List<Claim>
        {
            new Claim(AuthenticationPoliciesConsts.DefaultUserAuth, user.UserName)
        };
        var indentity = new ClaimsIdentity(claims, claimsSchema);
        return new ClaimsPrincipal(indentity);
    }

    public User? TryToGetUserAccountDetails(HttpContext httpContext)
    {
        User? user;
        var userIdentity = httpContext.User.Claims.FirstOrDefault(x => x.Type == AuthenticationPoliciesConsts.DefaultUserAuth);
        if (userIdentity is null)
            return null;

        try
        {
            user = _usersRepository.Users
                .Where(u => u.UserName == userIdentity.Value)
                .SingleOrDefault();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return null;
        } 

        return user;
    }
}
