using BookReviewsAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewsAPI.Controllers
{
    [ApiController]
    [Route("books")]
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
        public ActionResult GetBookById([FromRoute(Name="id")]int id)
        {
            var result = _bookRepository.GetById(id);
            return result is not null ? Ok(_bookRepository.GetById(id)) : NoContent();
        }

    }
}
