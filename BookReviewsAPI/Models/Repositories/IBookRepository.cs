namespace BookReviewsAPI.Models.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book GetById(int id);
        void Delete(int id);
        void Update(Book book);
    }
}
