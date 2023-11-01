using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReviews.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [Authorize(Policy = AuthenticationPoliciesConsts.DefaultUserAuth)]
    public class UserController: ControllerBase
    {
        private readonly IClaimsHelper _userClaimsHelper;
        private readonly BookReviewsDbContext _bookReviewsDbContext;
        private readonly ILogger<UserController> _logger;
        private readonly ImageSourcePathMapper _imageSourcePathMapper;
        public UserController(
            IClaimsHelper userClaimsHelper,
            BookReviewsDbContext bookReviewsDbContext,
            ILogger<UserController> logger, 
            ImageSourcePathMapper imageSourcePathMapper)
        {
            _userClaimsHelper = userClaimsHelper;
            _bookReviewsDbContext = bookReviewsDbContext;
            _logger = logger;
            _imageSourcePathMapper = imageSourcePathMapper;
        }

        [HttpGet]
        public ActionResult<User> GetUserData()
        {
            var user = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);

            if (user is null)
                return Unauthorized();

            var userData = _bookReviewsDbContext.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.LikedBooks)
                .FirstOrDefault();
            
            if (userData is null)
            {
                _logger.LogError($"There is disparity between {nameof(_userClaimsHelper)} (data: {user}) and {nameof(_bookReviewsDbContext)} (data: {userData})");
                return StatusCode(500);
            }

            if (userData.LikedBooks is not null)
            {
                _imageSourcePathMapper.MapUsersLikedBooksSourcePahts(userData);
            }

            _imageSourcePathMapper.MapUserProfilePictureToPath(userData);

            return Ok(userData);
        }
    }
}
