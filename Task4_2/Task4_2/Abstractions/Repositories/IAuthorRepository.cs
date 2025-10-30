using Task4_2.Models;

namespace Task4_2.Abstractions.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetByIdWithBooksAsync(int id);
    Task<IEnumerable<Author>> FindByNameAsync(string name);
}