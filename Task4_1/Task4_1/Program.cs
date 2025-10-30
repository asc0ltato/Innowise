using FluentValidation;
using Task4_1.Abstractions.Repositories;
using Task4_1.Abstractions.Services;
using Task4_1.DTOs;
using Task4_1.Infrastructure.Services;
using Task4_1.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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