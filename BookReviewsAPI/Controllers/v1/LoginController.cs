using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Authentication.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BookReviewsAPI.Models;
using BC = BCrypt.Net.BCrypt;
using BookReviewsAPI.Authentication.Helpers;

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
        public LoginController(
            ILogger<LoginController> logger, 
            IUserAuthenticationHelper userAuthenticationHelper, 
            IClaimsHelper claimsHelper
            )
        {
            _logger = logger;
            _userAuthenticationHelper = userAuthenticationHelper;
            _claimsHelper = claimsHelper;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {
            if (!_userAuthenticationHelper.IsAuthenticatedUser(user))
                return Unauthorized();

            var claimsSchema = AuthenticationSchemasConsts.DefaultSchema;
            var principal = _claimsHelper.GenerateUserClaimsPrincipal(user, claimsSchema);

            HttpContext.SignInAsync(claimsSchema, principal);
            _logger.LogInformation($"Logged in user with username={user.UserName}");

            return Ok();
        }
    }
}
