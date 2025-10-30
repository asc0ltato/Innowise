using Microsoft.AspNetCore.Mvc;
using Task4_1.Abstractions.Services;
using Task4_1.DTOs;

namespace Task4_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthor(int authorId)
    {
        var books = await _bookService.GetBooksByAuthorIdAsync(authorId);
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(BookRequestDto bookDto)
    {
        var book = await _bookService.CreateBookAsync(bookDto);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, BookRequestDto bookDto)
    {
        var book = await _bookService.UpdateBookAsync(id, bookDto);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}