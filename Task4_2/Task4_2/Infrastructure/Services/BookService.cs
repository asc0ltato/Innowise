using FluentValidation;
using Task4_2.Abstractions.Repositories;
using Task4_2.Abstractions.Services;
using Task4_2.DTOs;
using Task4_2.Models;

namespace Task4_2.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<BookRequestDto> _validator;

    public BookService(
        IBookRepository bookRepository, 
        IAuthorRepository authorRepository,
        IValidator<BookRequestDto> validator)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(b => MapToDto(b, b.Author));
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return book == null ? null : MapToDto(book, book.Author);
    }

    public async Task<IEnumerable<BookDto>> GetByAuthorIdAsync(int authorId)
    {
        var books = await _bookRepository.GetBooksByAuthorIdAsync(authorId);
        return books.Select(b => MapToDto(b, b.Author));
    }

    public async Task<BookDto> CreateAsync(BookRequestDto bookDto)
    {
        var validationResult = await _validator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
        }

        var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId);
        if (!authorExists)
            throw new ArgumentException($"Author with ID {bookDto.AuthorId} does not exist");

        var book = new Book
        {
            Title = bookDto.Title,
            PublishedYear = bookDto.PublishedYear,
            AuthorId = bookDto.AuthorId
        };

        var createdBook = await _bookRepository.CreateAsync(book);
        var author = await _authorRepository.GetByIdAsync(createdBook.AuthorId);
        return MapToDto(createdBook, author!);
    }

    public async Task<BookDto?> UpdateAsync(int id, BookRequestDto bookDto)
    {
        var validationResult = await _validator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
        }

        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null) return null;

        var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId);
        if (!authorExists)
            throw new ArgumentException($"Author with ID {bookDto.AuthorId} does not exist");

        existingBook.Title = bookDto.Title;
        existingBook.PublishedYear = bookDto.PublishedYear;
        existingBook.AuthorId = bookDto.AuthorId;

        var updatedBook = await _bookRepository.UpdateAsync(existingBook);
        var author = await _authorRepository.GetByIdAsync(updatedBook.AuthorId);
        return MapToDto(updatedBook, author!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _bookRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<BookDto>> GetPublishedAfterAsync(int year)
    {
        var books = await _bookRepository.GetBooksPublishedAfterAsync(year);
        return books.Select(b => MapToDto(b, b.Author));
    }

    private static BookDto MapToDto(Book book, Author author)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId,
            AuthorName = author.Name
        };
    }
}