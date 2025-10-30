using Task4_1.Abstractions.Repositories;
using Task4_1.Abstractions.Services;
using Task4_1.DTOs;
using Task4_1.Models;

namespace Task4_1.Infrastructure.Services;

 public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            var bookDtos = new List<BookDto>();

            foreach (var book in books)
            {
                var author = await _authorRepository.GetByIdAsync(book.AuthorId);
                bookDtos.Add(MapToDto(book, author));
            }

            return bookDtos;
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;

            var author = await _authorRepository.GetByIdAsync(book.AuthorId);
            return MapToDto(book, author);
        }

        public async Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId)
        {
            var books = await _bookRepository.GetBooksByAuthorIdAsync(authorId);
            var author = await _authorRepository.GetByIdAsync(authorId);

            return books.Select(book => MapToDto(book, author));
        }

        public async Task<BookDto> CreateBookAsync(BookRequestDto bookDto)
        {
            var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId);
            if (!authorExists)
            {
                throw new ArgumentException($"Author with ID {bookDto.AuthorId} does not exist.");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                PublishedYear = bookDto.PublishedYear,
                AuthorId = bookDto.AuthorId
            };

            var createdBook = await _bookRepository.CreateAsync(book);
            var author = await _authorRepository.GetByIdAsync(createdBook.AuthorId);
            
            return MapToDto(createdBook, author);
        }

        public async Task<BookDto?> UpdateBookAsync(int id, BookRequestDto bookDto)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null) return null;

            var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId);
            if (!authorExists)
            {
                throw new ArgumentException($"Author with ID {bookDto.AuthorId} does not exist.");
            }

            existingBook.Title = bookDto.Title;
            existingBook.PublishedYear = bookDto.PublishedYear;
            existingBook.AuthorId = bookDto.AuthorId;

            var updatedBook = await _bookRepository.UpdateAsync(existingBook);
            var author = await _authorRepository.GetByIdAsync(updatedBook.AuthorId);
            
            return MapToDto(updatedBook, author);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        private static BookDto MapToDto(Book book, Author? author)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                PublishedYear = book.PublishedYear,
                AuthorId = book.AuthorId,
                AuthorName = author?.Name ?? "Unknown"
            };
        }
    }