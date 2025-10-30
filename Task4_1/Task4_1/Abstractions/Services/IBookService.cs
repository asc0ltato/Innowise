using Task4_1.DTOs;

namespace Task4_1.Abstractions.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId);
    Task<BookDto> CreateBookAsync(BookRequestDto bookDto);
    Task<BookDto?> UpdateBookAsync(int id, BookRequestDto bookDto);
    Task<bool> DeleteBookAsync(int id);
}