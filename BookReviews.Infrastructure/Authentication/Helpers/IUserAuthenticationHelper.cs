using BookReviews.Domain.Models.DataModels;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IUserAuthenticationHelper
{
    bool IsAuthenticatedUser(User user);
}
