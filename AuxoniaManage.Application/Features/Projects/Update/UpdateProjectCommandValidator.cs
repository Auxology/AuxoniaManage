using FluentValidation;

namespace AuxoniaManage.Application.Features.Projects.Update;

internal sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("First name must not exceed 256 characters.");
        
        RuleFor(x => x.Logo)
            .Must(x => x == null || x.Length <= 2 * 1024 * 1024) 
            .WithMessage("Avatar must not exceed 2 MB in size.")
            .Must(x => x == null || (x.ContentType == "image/png" || x.ContentType == "image/jpeg" || x.ContentType == "image/jpg"))
            .WithMessage("Avatar must be a valid image file (png, jpg, jpeg).");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Name) || x.Logo != null)
            .WithMessage("At least one of Name or Logo must be provided.");
    }
}