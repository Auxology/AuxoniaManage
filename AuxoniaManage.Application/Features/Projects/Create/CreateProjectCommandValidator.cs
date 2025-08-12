using FluentValidation;
using FluentValidation.Validators;

namespace AuxoniaManage.Application.Features.Projects.Create;

internal sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(256)
            .WithMessage("Project name must not exceed 256 characters.");

        RuleFor(x => x.Logo)
            .Must(x => x == null || x.Length <= 2 * 1024 * 1024) 
            .WithMessage("Avatar must not exceed 2 MB in size.")
            .Must(x => x == null || (x.ContentType == "image/png" || x.ContentType == "image/jpeg" || x.ContentType == "image/jpg"))
            .WithMessage("Avatar must be a valid image file (png, jpg, jpeg).");
    }
}