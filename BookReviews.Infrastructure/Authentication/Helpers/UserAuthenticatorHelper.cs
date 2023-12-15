using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs;
using BookReviews.Domain.Models.DTOs.ExposedDTOs;
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

    public bool IsAuthenticatedUser(UserLoginDTO user)
    {
        User? matchedUser = TryRetrivingUserFromDBContext(user);

        if (matchedUser is null)
            return false;

        return _passwordCryptographyHelper.VerifyPassword(user.Password, matchedUser.Password);
    }

    private User? TryRetrivingUserFromDBContext(UserLoginDTO user)
    {
        User? matchedUser = null;
        try
        {
            matchedUser = _bookReviewsDbContext.Users.SingleOrDefault(x => x.UserName == user.UserName);
            return matchedUser;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("There shouldn't be two users with same username: {errorMessage}", ex.Message);
            return null;
        }
    }

    public UserDTO? GetUser(UserLoginDTO user)
    {
        User? matchedUser = TryRetrivingUserFromDBContext(user);

        if (matchedUser is null)
            return null;

        var matchedUserMappedToDTO = new UserDTO(matchedUser, false);

        return matchedUserMappedToDTO;
    }
}
