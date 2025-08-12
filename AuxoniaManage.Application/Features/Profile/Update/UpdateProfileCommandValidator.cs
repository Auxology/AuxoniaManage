using FluentValidation;

namespace AuxoniaManage.Application.Features.Profile.Update;

internal sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage("First name must not exceed 256 characters.");
        
        RuleFor(x => x.LastName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage("Last name must not exceed 256 characters.");
        
        RuleFor(x => x.Avatar)
            .Must(x => x == null || x.Length <= 2 * 1024 * 1024) 
            .WithMessage("Avatar must not exceed 2 MB in size.")
            .Must(x => x == null || (x.ContentType == "image/png" || x.ContentType == "image/jpeg" || x.ContentType == "image/jpg"))
            .WithMessage("Avatar must be a valid image file (png, jpg, jpeg).");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.FirstName) || !string.IsNullOrEmpty(x.LastName) || x.Avatar != null)
            .WithMessage("At least one of FirstName, LastName, or Avatar must be provided.");
    }
}