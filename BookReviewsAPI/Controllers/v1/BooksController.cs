using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Domain.Models;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Domain.Models.DataModels;

namespace BookReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/books")]
    [ApiVersion("1.0")]
    [Authorize(Policy = AuthenticationPoliciesConsts.DefaultUserAuth)]
    public class BooksController : ControllerBase
    {
        //private readonly IBookRepository _bookRepository;
        private readonly IConfiguration _configuration;
        private readonly BookReviewsDbContext _bookReviewsDbContext;
        public BooksController(BookReviewsDbContext bookReviewsDbContext,
            IConfiguration configuration)
        {
            _bookReviewsDbContext = bookReviewsDbContext;   
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetAllBooks()
        {
            var books = _bookReviewsDbContext.Books;
            foreach(var book in books)
            {
                mapBookImageSourceToEndpointPath(book);
            }
            return Ok(books);
        }

        [HttpGet("{id:int}")]

        public ActionResult GetBookById([FromRoute(Name="id")] int id)
        { 
            Book? bookResult;
            try
            {
                bookResult = _bookReviewsDbContext.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Reviews)
                    .SingleOrDefault(b => b.Id == id);
            }
            catch(InvalidOperationException )
            {
                return NotFound();
            }

            if (bookResult is not null)
                mapBookImageSourceToEndpointPath(bookResult);

            return bookResult is not null ? Ok(bookResult) : NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private void mapBookImageSourceToEndpointPath(Book book)
        {
            if (book.Img is null)
                book.Img = "placeholder";

            book.Img = _configuration.GetSection("ImageEndpointPrefix").Value + book.Img + ".jpeg";
        }

        [HttpGet("img/{name}")]
        public ActionResult GetImageById([FromRoute(Name = "name")] string name)
        {
            var filePath = $"./Resources/Images/{name}";
            if(!System.IO.File.Exists(filePath))
            {
                filePath = $"./Resources/Images/placeholder.jpeg";
            }

            Byte[] buffer = System.IO.File.ReadAllBytes(filePath);
            return File(buffer, "image/jpeg");
        }
    }
}
