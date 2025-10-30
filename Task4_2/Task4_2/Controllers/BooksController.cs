using Microsoft.AspNetCore.Mvc;
using Task4_2.Abstractions.Services;
using Task4_2.DTOs;

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
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthor(int authorId)
    {
        var books = await _bookService.GetByAuthorIdAsync(authorId);
        return Ok(books);
    }

    [HttpGet("published-after/{year}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksPublishedAfter(int year)
    {
        var books = await _bookService.GetPublishedAfterAsync(year);
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(BookRequestDto bookDto)
    {
        var book = await _bookService.CreateAsync(bookDto);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, BookRequestDto bookDto)
    {
        var book = await _bookService.UpdateAsync(id, bookDto);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}