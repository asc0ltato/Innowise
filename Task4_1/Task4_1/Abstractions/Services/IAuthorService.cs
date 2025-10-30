using Task4_1.DTOs;

namespace Task4_1.Abstractions.Services;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto?> GetAuthorByIdAsync(int id);
    Task<AuthorWithBooksDto?> GetAuthorWithBooksAsync(int id);
    Task<AuthorDto> CreateAuthorAsync(AuthorRequestDto authorDto);
    Task<AuthorDto?> UpdateAuthorAsync(int id, AuthorRequestDto authorDto);
    Task<bool> DeleteAuthorAsync(int id);
}