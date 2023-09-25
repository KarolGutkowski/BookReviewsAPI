using BookReviewsAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BookReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/books")]
    [ApiVersion("1.0")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public ActionResult GetAllBooks()
        {
            return Ok(_bookRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public ActionResult GetBookById([FromRoute(Name="id")] int id)
        {
            var result = _bookRepository.GetById(id);
            return result is not null ? Ok(_bookRepository.GetById(id)) : NoContent();
        }

        [HttpGet("img/{id:int}")]
        public ActionResult GetBookImageById([FromRoute(Name = "id")] int id)
        {
            var filePath = $"./Resources/Images/book-{id}.jpeg";
            if(!System.IO.File.Exists(filePath))
            {
                return NoContent();
            }

            Byte[] buffer = System.IO.File.ReadAllBytes(filePath);
            return File(buffer, "image/jpeg");
        }
    }
}
