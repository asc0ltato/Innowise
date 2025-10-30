using Microsoft.EntityFrameworkCore;
using Task4_2.Abstractions.Repositories;
using Task4_2.Data;
using Task4_2.Models;

namespace Task4_2.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.Include(b => b.Author).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.AuthorId == authorId)
            .Include(b => b.Author)
            .ToListAsync();
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetBooksPublishedAfterAsync(int year)
    {
        return await _context.Books
            .Where(b => b.PublishedYear > year)
            .Include(b => b.Author)
            .ToListAsync();
    }
}