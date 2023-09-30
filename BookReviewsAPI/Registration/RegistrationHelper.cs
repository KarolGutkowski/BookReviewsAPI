using BookReviewsAPI.Models;
using BookReviewsAPI.Cryptography;

namespace BookReviewsAPI.Registration
{
    public class RegistrationHelper : IRegistrationHelper
    {
        private readonly BookReviewsDbContext _bookReviewsDbContext;
        private readonly IPasswordCryptographyHelper _passwordCryptographyHelper;
        public RegistrationHelper(BookReviewsDbContext bookReviewsDbContext, IPasswordCryptographyHelper passwordCryptographyHelper)
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
                Password = hashedPassword
            };

            if (_bookReviewsDbContext.Users.Any(x => x.UserName == username))
            {
                return false;
            }

            _bookReviewsDbContext.Users.Add(user);
            _bookReviewsDbContext.SaveChanges();

            return true;
        }
    }
}
