using Task4_2.Models;

namespace Task4_2.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
    Task<IEnumerable<Book>> GetBooksPublishedAfterAsync(int year);
}