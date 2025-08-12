using FluentValidation;

namespace AuxoniaManage.Application.Features.Onboarding.CreateWorkspaceWithMembership;

internal sealed class CreateWorkspaceWithMembershipCommandValidator : AbstractValidator<CreateWorkspaceWithMembershipCommand>
{
    public CreateWorkspaceWithMembershipCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 256 characters.");

        RuleFor(x => x.Logo)
            .Must(x => x == null || x.Length <= 2 * 1024 * 1024) 
            .WithMessage("Logo must not exceed 2 MB in size.")
            .Must(x => x == null || (x.ContentType == "image/png" || x.ContentType == "image/jpeg" || x.ContentType == "image/jpg"))
            .WithMessage("Logo must be a valid image file (png, jpg, jpeg).");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Name) || !string.IsNullOrEmpty(x.Description) || x.Logo != null)
            .WithMessage("At least one of Name, Description, or Logo must be provided.");
    }
}