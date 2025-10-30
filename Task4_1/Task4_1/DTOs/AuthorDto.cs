namespace Task4_1.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

public class AuthorWithBooksDto : AuthorDto
{
    public List<BookDto> Books { get; set; } = new();
}

public class AuthorRequestDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}