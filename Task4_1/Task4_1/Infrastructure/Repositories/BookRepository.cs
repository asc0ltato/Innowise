using Task4_1.Abstractions.Repositories;
using Task4_1.Models;

namespace Task4_1.Infrastructure.Services;

public class BookRepository : IBookRepository
{
    private static readonly List<Book> _books = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        return Task.FromResult(_books.AsEnumerable());
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        var books = _books.Where(b => b.AuthorId == authorId);
        return Task.FromResult(books);
    }

    public Task<Book> CreateAsync(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        return Task.FromResult(book);
    }

    public Task<Book> UpdateAsync(Book book)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.AuthorId = book.AuthorId;
        }
        return Task.FromResult(existingBook!);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _books.Remove(book);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_books.Any(b => b.Id == id));
    }
}