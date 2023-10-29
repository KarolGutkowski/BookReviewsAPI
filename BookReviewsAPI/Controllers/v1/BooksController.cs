using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Domain.Models;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Infrastructure.Authentication.Helpers;

namespace BookReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/books")]
    [ApiVersion("1.0")]
    [Authorize(Policy = AuthenticationPoliciesConsts.DefaultUserAuth)]
    public class BooksController : ControllerBase
    {
        //private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly IConfiguration _configuration;
        private readonly BookReviewsDbContext _bookReviewsDbContext;
        private readonly IClaimsHelper _userClaimsHelper;
        public BooksController(BookReviewsDbContext bookReviewsDbContext,
            IConfiguration configuration,
            ILogger<BooksController> logger,
            IClaimsHelper userClaimsHelper)
        {
            _bookReviewsDbContext = bookReviewsDbContext;
            _configuration = configuration;
            _logger = logger;
            _userClaimsHelper = userClaimsHelper;
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public ActionResult GetTest()
        {
            return Ok(_configuration.GetConnectionString("BooksReviews"));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllBooks()
        {
            var books = _bookReviewsDbContext.Books;
            foreach (var book in books)
            {
                MapBookImageSourceToEndpointPath(book);
            }
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public ActionResult GetBookById([FromRoute(Name = "id")] int id)
        {
            Book? bookResult;
            try
            {
                bookResult = _bookReviewsDbContext.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Reviews)
                    .SingleOrDefault(b => b.Id == id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            if (bookResult is not null)
                MapBookImageSourceToEndpointPath(bookResult);

            return bookResult is not null ? Ok(bookResult) : NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        private void MapBookImageSourceToEndpointPath(Book book)
        {
            if (book.Img is null)
                book.Img = "placeholder";

            book.Img = _configuration.GetSection("ImageEndpointPrefix").Value + book.Img + ".jpeg";
        }

        [HttpGet("img/{name}")]
        [AllowAnonymous]
        public ActionResult GetImageById([FromRoute(Name = "name")] string name)
        {
            var filePath = $"./Resources/Images/{name}";
            if (!System.IO.File.Exists(filePath))
            {
                filePath = $"./Resources/Images/placeholder.jpeg";
            }

            Byte[] buffer = System.IO.File.ReadAllBytes(filePath);
            return File(buffer, "image/jpeg");
        }

        [HttpGet("random")]
        [AllowAnonymous]
        public ActionResult GetRandomBook()
        {
            var result = _bookReviewsDbContext.Books
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();

            if (result is null)
            {
                return NoContent();
            }

            MapBookImageSourceToEndpointPath(result);
            return Ok(result);
        }


        [HttpPost("liked/{id:int}")]
        public ActionResult AddLikedBook([FromRoute(Name = "id")] int id)
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);

            if (userAccount is null)
                return Unauthorized();

            var user = _bookReviewsDbContext.Users
                .Where(u => u.Id == userAccount.Id)
                .Include(u => u.LikedBooks)
                .FirstOrDefault();

            if (user is null)
                return StatusCode(501);

            var book = _bookReviewsDbContext.Books
                .Where(b => b.Id == id)
                .FirstOrDefault();

            if (book is null)
            {
                return NotFound();
            }

            var bookLikes = book.LikedByUsers;
            if (user.LikedBooks is null)
            {
                user.LikedBooks = new List<Book>();
            }

            user.LikedBooks.Add(book);

            _bookReviewsDbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("liked")]
        public ActionResult GetLikedBooks()
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);

            if (userAccount is null)
                return Unauthorized();

            var user = _bookReviewsDbContext.Users
                .Where(u => u.Id == userAccount.Id)
                .Include(u => u.LikedBooks)
                .FirstOrDefault();

            if (user is null)
                return StatusCode(501);

            if (user.LikedBooks is null)
                return NoContent();

            var userLikedBooks = user.LikedBooks;
            return Ok(userLikedBooks);
        }

        [HttpGet("liked/{id:int}")]
        public ActionResult GetLikedBooks([FromRoute(Name ="id")] int id)
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);

            if (userAccount is null)
                return Unauthorized();

            var user = _bookReviewsDbContext.Users
                .Where(u => u.Id == userAccount.Id)
                .Include(u => u.LikedBooks)
                .FirstOrDefault();

            if (user is null)
                return StatusCode(501);

            if (user.LikedBooks is null)
                return NoContent();

            var userLikedBooks = user.LikedBooks;

            var bookWithGivenId = userLikedBooks.FirstOrDefault(x => x.Id == id);

            if (bookWithGivenId is null)
                return NoContent();

            return Ok(bookWithGivenId);
        }

        [HttpPatch("liked/remove/{id:int}")]
        public ActionResult RemoveFromUserLikedBooks([FromRoute(Name = "id")] int id)
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);

            if (userAccount is null)
                return Unauthorized();

            var user = _bookReviewsDbContext.Users
                .Where(u => u.Id == userAccount.Id)
                .Include(u => u.LikedBooks)
                .FirstOrDefault();

            if (user is null)
                return StatusCode(501);

            if (user.LikedBooks is null)
                return Ok();

            var book = _bookReviewsDbContext.Books
               .Where(b => b.Id == id)
               .FirstOrDefault();

            if (book is null)
            {
                return Ok();
            }

            user.LikedBooks.Remove(book);
            _bookReviewsDbContext.SaveChanges();
            return Ok();
        }     
    }
}
