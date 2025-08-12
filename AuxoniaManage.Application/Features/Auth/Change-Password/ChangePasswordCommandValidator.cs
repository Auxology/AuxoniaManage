using FluentValidation;

namespace AuxoniaManage.Application.Features.Auth.Change_Password;

internal sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
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
            
        RuleFor(x => x)
            .Must(x => x.NewPassword != x.OldPassword)
            .WithMessage("New password must be different from the current password.")
            .When(x => !string.IsNullOrEmpty(x.NewPassword) && !string.IsNullOrEmpty(x.OldPassword));
    }
}