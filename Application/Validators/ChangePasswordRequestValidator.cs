using Application.DTOs.Request.Auth;
using FluentValidation;

namespace Application.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .Matches("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage("Password must be at least 8 characters and include uppercase, lowercase, number and special character");
        }
    }
}