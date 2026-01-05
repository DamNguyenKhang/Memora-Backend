using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validators
{
    public class SignUpUserRequestValidator : AbstractValidator<SignUpUserRequest>
    {
        public SignUpUserRequestValidator()
        {
            // Username
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Matches("^[a-zA-Z0-9_]{6,20}$")
                .WithMessage("Username must be 6–20 characters and contain only letters, numbers, and underscores");

            // Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage("Password must be at least 8 characters and include uppercase, lowercase, number and special character");
        }
    }
}