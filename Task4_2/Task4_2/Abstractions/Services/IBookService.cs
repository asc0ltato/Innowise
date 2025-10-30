using Task4_2.DTOs;

namespace Task4_2.Abstractions.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetByAuthorIdAsync(int authorId);
    Task<BookDto> CreateAsync(BookRequestDto bookDto);
    Task<BookDto?> UpdateAsync(int id, BookRequestDto bookDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<BookDto>> GetPublishedAfterAsync(int year);
}