﻿namespace Task4_2.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}