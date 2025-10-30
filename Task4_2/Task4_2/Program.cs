using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Task4_2.Abstractions.Repositories;
using Task4_2.Abstractions.Services;
using Task4_2.Data;
using Task4_2.DTOs;
using Task4_2.Infrastructure.Repositories;
using Task4_2.Infrastructure.Services;
using Task4_2.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Register services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

// Register validators
builder.Services.AddScoped<IValidator<AuthorRequestDto>, AuthorRequestDtoValidator>();
builder.Services.AddScoped<IValidator<BookRequestDto>, BookRequestDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();