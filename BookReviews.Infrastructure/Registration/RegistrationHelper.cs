using BookReviews.Infrastructure.Cryptography;
using BookReviewsAPI.Registration;
using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;

namespace BookReviews.Infrastructure.Registration;

public class RegistrationHelper : IRegistrationHelper
{
    private readonly IUsersRepository _bookReviewsDbContext;
    private readonly IPasswordCryptographyHelper _passwordCryptographyHelper;
    public RegistrationHelper(IUsersRepository bookReviewsDbContext, IPasswordCryptographyHelper passwordCryptographyHelper)
    {
        _bookReviewsDbContext = bookReviewsDbContext;
        _passwordCryptographyHelper = passwordCryptographyHelper;
    }

    public bool TryToRegisterUser(string username, string password)
    {
        var hashedPassword = _passwordCryptographyHelper.GenerateHash(password);
        var user = new User()
        {
            UserName = username,
            Password = hashedPassword,
            ProfileImage = String.Empty
        };

        if (_bookReviewsDbContext.Users.Any(x => x.UserName == username))
        {
            return false;
        }

        _bookReviewsDbContext.Users.Add(user);
        _bookReviewsDbContext.FinishRegistration();

        return true;
    }
}
