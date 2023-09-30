using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Authentication.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BookReviewsAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace BookReviewsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/login")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class LoginController: ControllerBase
    {
        private readonly BookReviewsDbContext _bookReviewsDbContext;
        public LoginController(BookReviewsDbContext bookReviewsDbContext)
        {
            _bookReviewsDbContext = bookReviewsDbContext;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {

            var isUser = HasCorrectUserCredentials(user);

            if(!isUser)
                return Unauthorized();


            var claims = new List<Claim>();
            claims.Add(new Claim(AuthenticationPoliciesConsts.DefaultUserAuth, user.UserName));
            var indentity = new ClaimsIdentity(claims, AuthenticationSchemasConsts.DefaultSchema);
            var principal = new ClaimsPrincipal(indentity);

            HttpContext.SignInAsync(AuthenticationSchemasConsts.DefaultSchema, principal);

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public bool HasCorrectUserCredentials(User user)
        {
            User? matchedUser = null;
            try
            {
                matchedUser = _bookReviewsDbContext.Users.SingleOrDefault((x) => x.UserName == user.UserName);
            }catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            if (matchedUser is null)
                return false;

            return BC.Verify(user.Password, matchedUser.Password, enhancedEntropy: true);
        }

    }
}
