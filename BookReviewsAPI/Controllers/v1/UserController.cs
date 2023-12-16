using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs.ExposedDTOs;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace BookReviews.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [Authorize(Policy = AuthenticationPoliciesConsts.DefaultUserAuth)]
    public class UserController : ControllerBase
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

        [HttpPatch]
        [Route("profile-image")]
        public async Task<ActionResult> UpdateUserImage(IFormFile file)
        {
            var user = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);
            if (user is null)
                return Unauthorized();

            var userData = _bookReviewsDbContext.Users
                .Where(u => u.Id == user.Id)
                .FirstOrDefault();

            if (userData is null)
                return StatusCode(500);

            var splittedFileName = file.FileName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (splittedFileName.Length != 2)
                return BadRequest("bad media type");

            var imageFileName = "user-" + user.Id + "." + splittedFileName[1];

            var localPath = _imageSourcePathMapper.MapToServerPath(imageFileName);
            using var stream = System.IO.File.Create(localPath);
            await file.CopyToAsync(stream);

            userData.ProfileImage = imageFileName;
            _bookReviewsDbContext.SaveChanges();

            var userDataAsDTO = new UserDTO(userData, mapLikedBooks: false);

            return Ok(userDataAsDTO);
        }
    }
}
