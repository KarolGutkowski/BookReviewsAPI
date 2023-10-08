using BookReviews.Domain.Models.DataModels;
using BookReviewsAPI.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/register")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class RegistrationController: ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IRegistrationHelper _registrationHelper;
        public RegistrationController(
            ILogger<RegistrationController> logger, 
            IRegistrationHelper registrationHelper
            )
        {
            _logger = logger;
            _registrationHelper = registrationHelper;
        }


        [HttpPost("{username}/{password}")]
        public ActionResult Register([FromRoute] string username, string password)
        {
            return _registrationHelper.TryToRegisterUser(username, password)? Ok(): Conflict();
        }


        [HttpPost]
        public ActionResult Register([FromBody] User user)
        {
            return _registrationHelper.TryToRegisterUser(user.UserName, user.Password) ? Ok() : Conflict();
        }
    }
}
