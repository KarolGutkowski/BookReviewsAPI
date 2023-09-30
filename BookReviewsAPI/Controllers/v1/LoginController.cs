using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Authentication.Schemas;
using Microsoft.AspNetCore.Authentication;

namespace BookReviewsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/login")]
    [ApiVersion("1.0")]
    public class LoginController: ControllerBase
    {
        
        [HttpGet]
        public ActionResult LoginDefaultUser()
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(AuthenticationPoliciesConsts.DefaultUserAuth, "placeholder_name"));

            var indentity = new ClaimsIdentity(claims, AuthenticationSchemasConsts.DefaultSchema);
            var principal = new ClaimsPrincipal(indentity);
            HttpContext.SignInAsync(AuthenticationSchemasConsts.DefaultSchema, principal);

            return Ok();
        }

    }
}
