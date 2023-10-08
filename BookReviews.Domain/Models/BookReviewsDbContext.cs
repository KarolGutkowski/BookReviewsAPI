using BookReviews.Domain.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookReviews.Domain.Models
{
    public class BookReviewsDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public BookReviewsDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BookReviewsDbContext(
            DbContextOptions<BookReviewsDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BooksReviews"));
        }
    }
}