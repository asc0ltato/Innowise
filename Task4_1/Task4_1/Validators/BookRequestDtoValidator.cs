using FluentValidation;
using Task4_1.DTOs;

namespace Task4_1.Validators;

public class BookRequestDtoValidator : AbstractValidator<BookRequestDto>
{
    public BookRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Book title is required.")
            .MaximumLength(70).WithMessage("Book title must not exceed 70 characters.");

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1000, DateTime.Now.Year)
            .WithMessage($"Published year must be between 1000 and {DateTime.Now.Year}.");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
    }
}