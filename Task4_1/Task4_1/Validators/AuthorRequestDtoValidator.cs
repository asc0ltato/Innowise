using FluentValidation;
using Task4_1.DTOs;

namespace Task4_1.Validators;

public class AuthorRequestDtoValidator : AbstractValidator<AuthorRequestDto>
{
    public AuthorRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Author name is required.")
            .MaximumLength(50).WithMessage("Author name must not exceed 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.")
            .GreaterThan(DateTime.Now.AddYears(-130)).WithMessage("Author cannot be older than 130 years.");
    }
}