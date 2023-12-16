using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Domain.Models;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Mappers;
using BookReviews.Domain.Models.DTOs.ExposedDTOs;

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
        private readonly ImageSourcePathMapper _imageSourcePathMapper;
        public BooksController(BookReviewsDbContext bookReviewsDbContext,
            IConfiguration configuration,
            ILogger<BooksController> logger,
            IClaimsHelper userClaimsHelper, 
            ImageSourcePathMapper imageSourcePathMapper)
        {
            _bookReviewsDbContext = bookReviewsDbContext;
            _configuration = configuration;
            _logger = logger;
            _userClaimsHelper = userClaimsHelper;
            _imageSourcePathMapper = imageSourcePathMapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllBooks()
        {
            _logger.LogInformation("User requested all books");
            var books = _bookReviewsDbContext.Books;
            foreach (var book in books)
            {
                _imageSourcePathMapper.MapBookImageSourceToEndpointPath(book);
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
                    .ThenInclude(r => r.User)
                    .SingleOrDefault(b => b.Id == id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            if (bookResult is null)
                return NoContent();

            _imageSourcePathMapper.MapBookImageSourceToEndpointPath(bookResult);

            var bookResultAsDTO = new BookDTO(bookResult, true, true, false);

            decimal sum = 0m;
            int count = 0;  
            foreach(var review in bookResultAsDTO.Reviews)
            {
                sum += review.Rating;
                count++;
            }
            if(count != 0)
                bookResultAsDTO.AverageRating = sum/count;

            return Ok(bookResultAsDTO);
        }

        [HttpGet("img/{name}")]
        [AllowAnonymous]
        public ActionResult GetImageById([FromRoute(Name = "name")] string name)
        {
            var filePath = _imageSourcePathMapper.MapToServerPath(name);
            if (!System.IO.File.Exists(filePath))
            {
                filePath = _imageSourcePathMapper.MapToServerPath("placeholder.jpeg");
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

            _imageSourcePathMapper.MapBookImageSourceToEndpointPath(result);
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
        
        [HttpGet("search/{searchPhrase}")]
        [AllowAnonymous]
        public ActionResult SearchBooks([FromRoute] string searchPhrase)
        {
            var searchPhraseCaseInsensitive = searchPhrase.ToLower();

            var booksFound = _bookReviewsDbContext.Books
                .Include(b => b.Authors)
                .AsEnumerable()
                .Where(book =>
                {
                    if (book.Title.ToLower().Contains(searchPhraseCaseInsensitive))
                        return true;

                    foreach (var author in book.Authors)
                    {
                        var authorNameAndSurname = String.Concat(author.FirstName, " ", author.LastName);
                        if (authorNameAndSurname.ToLower().Contains(searchPhraseCaseInsensitive))
                            return true;
                    }

                    return false;
                });

            foreach(var book in booksFound)
            {
                _imageSourcePathMapper.MapBookImageSourceToEndpointPath(book);
            }

            return Ok(booksFound);
        }
    }
}
