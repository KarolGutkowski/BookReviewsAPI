using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace BookReviewsAPI.Models.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        public BookRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection("Data Source = localhost; Initial Catalog = BookReviewsDB; Integrated Security = True");
            _configuration = configuration;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAll()
        {
            string getAllQuery = "SELECT B.Id, B.Title, B.[year], A.Id As AuthorId, A.first_name As FirstName, A.last_name As LastName, A.date_of_birth As DateOfBirth FROM Books B INNER JOIN Book_Authors Ba ON Ba.book_id = B.id INNER JOIN Authors A ON Ba.author_id = A.id";

            var books = _dbConnection.Query<Book, Author, Book>(getAllQuery, (book, author) =>
            {
                if (book.Authors is null)
                {
                    book.Authors = new List<Author>();
                }
                book.Authors.Add(author);
                return book;
            }, splitOn: "AuthorId");

            return books;
        }

        public Book? GetById(int id)
        {
            string getBookByIdQuery =
                "SELECT B.* FROM Books AS B WHERE Id = @Id;"+
                "SELECT A.Id As AuthorId,A.first_name AS FirstName,A.last_name AS LastName,A.date_of_birth AS DateOfBirth FROM Authors AS A WHERE A.id IN(SELECT author_id FROM Book_Authors WHERE book_id = @Id)";

            using (var multi = _dbConnection.QueryMultiple(getBookByIdQuery, new { Id=id }))
            {
                var book = multi.Read<Book>().FirstOrDefault();
                if(book is null)
                    return null;
                var authors = multi.Read<Author>()?.ToList();
                if(authors is not null)
                    book.Authors = new List<Author>(authors);
                return book;
            }
        }

        public void Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
