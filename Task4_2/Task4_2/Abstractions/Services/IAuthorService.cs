using Task4_2.DTOs;

namespace Task4_2.Abstractions.Services;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorWithBooksDto?> GetWithBooksAsync(int id);
    Task<AuthorDto> CreateAsync(AuthorRequestDto authorDto);
    Task<AuthorDto?> UpdateAsync(int id, AuthorRequestDto authorDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AuthorDto>> FindByNameAsync(string name);
}