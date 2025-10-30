using Task4_1.Abstractions.Repositories;
using Task4_1.Abstractions.Services;
using Task4_1.DTOs;
using Task4_1.Models;

namespace Task4_1.Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(MapToDto);
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        return author == null ? null : MapToDto(author);
    }

    public async Task<AuthorWithBooksDto?> GetAuthorWithBooksAsync(int id)
    {
        var author = await _authorRepository.GetByIdWithBooksAsync(id);
        if (author == null) return null;

        var books = await _bookRepository.GetBooksByAuthorIdAsync(id);
        
        return new AuthorWithBooksDto
        {
            Id = author.Id,
            Name = author.Name,
            DateOfBirth = author.DateOfBirth,
            Books = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                PublishedYear = b.PublishedYear,
                AuthorId = b.AuthorId,
                AuthorName = author.Name
            }).ToList()
        };
    }

    public async Task<AuthorDto> CreateAuthorAsync(AuthorRequestDto  authorDto)
    {
        var author = new Author
        {
            Name = authorDto.Name,
            DateOfBirth = authorDto.DateOfBirth
        };

        var createdAuthor = await _authorRepository.CreateAsync(author);
        return MapToDto(createdAuthor);
    }

    public async Task<AuthorDto?> UpdateAuthorAsync(int id, AuthorRequestDto  authorDto)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(id);
        if (existingAuthor == null) return null;

        existingAuthor.Name = authorDto.Name;
        existingAuthor.DateOfBirth = authorDto.DateOfBirth;

        var updatedAuthor = await _authorRepository.UpdateAsync(existingAuthor);
        return MapToDto(updatedAuthor);
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var authorBooks = await _bookRepository.GetBooksByAuthorIdAsync(id);
        if (authorBooks.Any())
        {
            return false;
        }

        return await _authorRepository.DeleteAsync(id);
    }

    private static AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            DateOfBirth = author.DateOfBirth
        };
    }
}