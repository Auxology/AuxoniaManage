using FluentValidation;

namespace AuxoniaManage.Application.Features.Auth.Reset_Password;

internal sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(12)
            .WithMessage("Password must be at least 12 characters long.")
            .MaximumLength(256)
            .WithMessage("Password must not exceed 256 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
    }
}