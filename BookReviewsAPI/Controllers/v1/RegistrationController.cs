using BookReviewsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace BookReviewsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/register")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class RegistrationController: ControllerBase
    {
        private readonly BookReviewsDbContext _bookReviewsDbContext;

        public RegistrationController(BookReviewsDbContext bookReviewsDbContext)
        {
            _bookReviewsDbContext = bookReviewsDbContext;
        }

        
        [HttpPost("{username}/{password}")]
        public ActionResult Register([FromRoute] string username, string password)
        {
            return TryToRegisterUser(username, password);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult TryToRegisterUser(string username, string password)
        {
            var hashedPassword = BC.EnhancedHashPassword(password);
            var user = new User()
            {
                UserName = username,
                Password = hashedPassword
            };

            if (_bookReviewsDbContext.Users.Any(x => x.UserName == username))
            {
                return Conflict("username already taken");
            }

            _bookReviewsDbContext.Users.Add(user);
            _bookReviewsDbContext.SaveChanges();

            return Ok(user);
        }


        [HttpPost]
        public ActionResult Register([FromBody] User user)
        {
            return TryToRegisterUser(user.UserName, user.Password);
        }
    }
}
