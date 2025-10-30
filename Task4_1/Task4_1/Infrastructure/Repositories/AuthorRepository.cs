using Task4_1.Abstractions.Repositories;
using Task4_1.Models;

namespace Task4_1.Infrastructure.Services;

public class AuthorRepository : IAuthorRepository
{
    private static readonly List<Author> _authors = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Author>> GetAllAsync()
    {
        return Task.FromResult(_authors.AsEnumerable());
    }

    public Task<Author?> GetByIdAsync(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(author);
    }

    public Task<Author?> GetByIdWithBooksAsync(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(author);
    }

    public Task<Author> CreateAsync(Author author)
    {
        author.Id = _nextId++;
        _authors.Add(author);
        return Task.FromResult(author);
    }

    public Task<Author> UpdateAsync(Author author)
    {
        var existingAuthor = _authors.FirstOrDefault(a => a.Id == author.Id);
        if (existingAuthor != null)
        {
            existingAuthor.Name = author.Name;
            existingAuthor.DateOfBirth = author.DateOfBirth;
        }
        return Task.FromResult(existingAuthor!);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author != null)
        {
            _authors.Remove(author);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_authors.Any(a => a.Id == id));
    }
}