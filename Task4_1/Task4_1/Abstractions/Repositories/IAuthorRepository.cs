using Task4_1.Models;

namespace Task4_1.Abstractions.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetByIdWithBooksAsync(int id);
}