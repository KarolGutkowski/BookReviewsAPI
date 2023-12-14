using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs;
using BookReviews.Infrastructure.Cryptography;
using Microsoft.Extensions.Logging;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public class UserAuthenticatonHelper : IUserAuthenticationHelper
{
    private readonly BookReviewsDbContext _bookReviewsDbContext;
    private readonly ILogger<UserAuthenticatonHelper> _logger;
    private readonly IPasswordCryptographyHelper _passwordCryptographyHelper;
    public UserAuthenticatonHelper(
        BookReviewsDbContext bookReviewsDbContext,
        ILogger<UserAuthenticatonHelper> logger,
        IPasswordCryptographyHelper passwordCryptographyHelper
        )
    {
        _bookReviewsDbContext = bookReviewsDbContext;
        _logger = logger;
        _passwordCryptographyHelper = passwordCryptographyHelper;
    }

    public bool IsAuthenticatedUser(Domain.Models.DTOs.UserLoginDTO user)
    {
        Domain.Models.DataModels.User? matchedUser = null;
        try
        {
            matchedUser = _bookReviewsDbContext.Users.SingleOrDefault(x => x.UserName == user.UserName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("There shouldn't be two users with same username: {errorMessage}", ex.Message);
            return false;
        }

        if (matchedUser is null)
            return false;

        return _passwordCryptographyHelper.VerifyPassword(user.Password, matchedUser.Password);
    }
}
