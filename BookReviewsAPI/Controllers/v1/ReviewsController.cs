using BookReviews.Domain.Models;
using BookReviews.Domain.Models.DataModels;
using BookReviews.Domain.Models.DTOs;
using BookReviews.Domain.Models.DTOs.ExposedDTOs;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Authentication.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReviews.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/reviews")]
    [ApiVersion("1.0")]
    [Authorize(Policy = AuthenticationPoliciesConsts.DefaultUserAuth)]
    public class ReviewsController: ControllerBase
    {
        public BookReviewsDbContext _bookReviewsDbContext { get; }
        private IClaimsHelper _userClaimsHelper { get; }
        public ReviewsController(
            BookReviewsDbContext bookReviewsDbContext,
            IClaimsHelper userClaimsHelper)
        {
            _bookReviewsDbContext = bookReviewsDbContext;
            _userClaimsHelper = userClaimsHelper;
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] BookReviewDTO reviewData)
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);
            if (userAccount is null)
                return Unauthorized();

            var book = _bookReviewsDbContext.Books
                .Include(b => b.Reviews)
                .Where(b => b.Id == reviewData.BookId)
                .SingleOrDefault();

            if(book is null)
                return NotFound();

            var correspondingUserFromDBContext = _bookReviewsDbContext.Users
                .Where(u => u.Id == userAccount.Id)
                .Single();

            var review = new Review()
            {
                Book = book,
                User = correspondingUserFromDBContext,
                CreatedAt = reviewData.Date,
                Rating = reviewData.Rating,
                Content = reviewData.Content
            };

            _bookReviewsDbContext.Reviews.Add(review);
            _bookReviewsDbContext.SaveChanges();

            return Ok();
        }

        [Route("book/{bookId:int}")]
        [HttpGet]
        public IActionResult UserRevieweOnTheBook([FromRoute] int bookId)
        {
            var userAccount = _userClaimsHelper.TryToGetUserAccountDetails(HttpContext);
            if (userAccount is null)
                return Unauthorized();

            var book = _bookReviewsDbContext.Books
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .Where(b => b.Id == bookId)
                .SingleOrDefault();


            if (book is not null && book.Reviews.Any(r => r.User.Id == userAccount.Id))
            {
                var review = book.Reviews
                    .Where(r => r.User.Id == userAccount.Id)
                    .First();

                var reviewDTO = new ReviewDTO(review, book: null);

                return Ok(reviewDTO);
            }

            return NoContent();
        }
    }
}
