using FluentValidation;

namespace AuxoniaManage.Application.Features.Workspace.Update;

internal sealed class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateWorkspaceCommand>
{
    public UpdateWorkspaceCommandValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace ID is required.");
            
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Workspace name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));
            
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Workspace description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}