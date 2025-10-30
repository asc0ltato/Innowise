using Microsoft.AspNetCore.Mvc;
using Task4_1.Abstractions.Services;
using Task4_1.DTOs;

namespace Task4_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpGet("{id}/with-books")]
    public async Task<ActionResult<AuthorWithBooksDto>> GetAuthorWithBooks(int id)
    {
        var author = await _authorService.GetAuthorWithBooksAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorRequestDto authorDto)
    {
        var author = await _authorService.CreateAuthorAsync(authorDto);
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, AuthorRequestDto authorDto)
    {
        var author = await _authorService.UpdateAuthorAsync(id, authorDto);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var result = await _authorService.DeleteAuthorAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
    