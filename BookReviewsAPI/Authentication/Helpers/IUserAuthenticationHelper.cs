using BookReviewsAPI.Models;

namespace BookReviewsAPI.Authentication.Helpers
{
    public interface IUserAuthenticationHelper
    {
        bool IsAuthenticatedUser(User user);
    }
}
