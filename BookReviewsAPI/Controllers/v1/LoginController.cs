using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Authentication.Schemas;
using BookReviews.Domain.Models.DTOs;
using BookReviews.Domain.Models;

namespace BookReviewsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/login")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserAuthenticationHelper _userAuthenticationHelper;
        private readonly IClaimsHelper _claimsHelper;
        private readonly IUsersRepository _usersRepository;
        public LoginController(
            ILogger<LoginController> logger, 
            IUserAuthenticationHelper userAuthenticationHelper, 
            IClaimsHelper claimsHelper,
            IUsersRepository usersRepository
            )
        {
            _logger = logger;
            _userAuthenticationHelper = userAuthenticationHelper;
            _claimsHelper = claimsHelper;
            _usersRepository = usersRepository;
        }

        [HttpPost]
        public ActionResult Login([FromBody] UserLoginDTO user)
        {
            if (!_userAuthenticationHelper.IsAuthenticatedUser(user))
                return Unauthorized();

            var userData = _userAuthenticationHelper.GetUser(user);

            var claimsSchema = AuthenticationSchemasConsts.DefaultSchema;
            var principal = _claimsHelper.GenerateUserClaimsPrincipal(userData, claimsSchema);

            HttpContext.SignInAsync(claimsSchema, principal);
            _logger.LogInformation("Logged in user with username={UserName}", user.UserName);


            if (userData is null)
            {
                return StatusCode(500);
            }

            return Ok(userData);
        }
    }
}
