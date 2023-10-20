using BookReviews.Domain.Models.DTOs;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IUserAuthenticationHelper
{
    bool IsAuthenticatedUser(UserDTO user);
}
