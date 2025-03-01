using FluentValidation;
using MyBaseProject.Application.DTOs.Requests;

namespace MyBaseProject.Application.Validators
{
    public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequestDto>
    {
        public AccountCreateRequestValidator()
        {
            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(50).WithMessage("Password must be at most 50 characters.");

            RuleFor(a => a.FirstName)
                .MaximumLength(50).WithMessage("FirstName must be at most 50 characters.");

            RuleFor(a => a.LastName)
                .MaximumLength(50).WithMessage("LastName must be at most 50 characters.");

            RuleFor(a => a.PhoneNumber)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage("Invalid phone number format.")
                .When(a => !string.IsNullOrEmpty(a.PhoneNumber));
        }
    }
}
