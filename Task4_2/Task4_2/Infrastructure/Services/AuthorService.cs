using FluentValidation;
using Task4_2.Abstractions.Repositories;
using Task4_2.Abstractions.Services;
using Task4_2.DTOs;
using Task4_2.Models;

namespace Task4_2.Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IValidator<AuthorRequestDto> _validator;

    public AuthorService(
        IAuthorRepository authorRepository, 
        IBookRepository bookRepository,
        IValidator<AuthorRequestDto> validator)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(MapToDto);
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        return author == null ? null : MapToDto(author);
    }

    public async Task<AuthorWithBooksDto?> GetWithBooksAsync(int id)
    {
        var author = await _authorRepository.GetByIdWithBooksAsync(id);
        if (author == null) return null;

        return new AuthorWithBooksDto
        {
            Id = author.Id,
            Name = author.Name,
            DateOfBirth = author.DateOfBirth,
            Books = author.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                PublishedYear = b.PublishedYear,
                AuthorId = b.AuthorId,
                AuthorName = author.Name
            }).ToList()
        };
    }

    public async Task<AuthorDto> CreateAsync(AuthorRequestDto authorDto)
    {
        var validationResult = await _validator.ValidateAsync(authorDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
        }

        var author = new Author
        {
            Name = authorDto.Name,
            DateOfBirth = authorDto.DateOfBirth
        };

        var createdAuthor = await _authorRepository.CreateAsync(author);
        return MapToDto(createdAuthor);
    }

    public async Task<AuthorDto?> UpdateAsync(int id, AuthorRequestDto authorDto)
    {
        var validationResult = await _validator.ValidateAsync(authorDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
        }

        var existingAuthor = await _authorRepository.GetByIdAsync(id);
        if (existingAuthor == null) return null;

        existingAuthor.Name = authorDto.Name;
        existingAuthor.DateOfBirth = authorDto.DateOfBirth;

        var updatedAuthor = await _authorRepository.UpdateAsync(existingAuthor);
        return MapToDto(updatedAuthor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _authorRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AuthorDto>> FindByNameAsync(string name)
    {
        var authors = await _authorRepository.FindByNameAsync(name);
        return authors.Select(MapToDto);
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