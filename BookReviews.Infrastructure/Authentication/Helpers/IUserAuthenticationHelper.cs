using BookReviews.Domain.Models.DTOs;
using BookReviews.Domain.Models.DTOs.ExposedDTOs;

namespace BookReviews.Infrastructure.Authentication.Helpers;

public interface IUserAuthenticationHelper
{
    bool IsAuthenticatedUser(UserLoginDTO user);
    UserDTO GetUser(UserLoginDTO user);
}
