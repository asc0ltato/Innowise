using Task4_1.Models;

namespace Task4_1.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
}