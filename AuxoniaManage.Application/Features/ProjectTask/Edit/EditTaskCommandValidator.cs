using FluentValidation;

namespace AuxoniaManage.Application.Features.ProjectTask.Edit;

internal sealed class EditTaskCommandValidator : AbstractValidator<EditTaskCommand>
{
    public EditTaskCommandValidator()
    {
        
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Task title must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Task description must not exceed 100 characters.");

        RuleFor(x => x.DeadlineAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.DeadlineAt.HasValue)
            .WithMessage("Deadline must be in the future.");
        
        RuleFor(x => x.Priority)
            .IsInEnum()
            .When(x => x.Priority.HasValue)
            .WithMessage("Invalid priority value.");
        
        RuleFor(x => x.Status) 
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid status value.");

        RuleFor(x => x.AssigneeIds)
            .NotNull()
            .WithMessage("Assignee IDs collection is required.")
            .Must(x => x.Count > 0)
            .WithMessage("At least one assignee is required.")
            .Must(x => x.All(id => !string.IsNullOrWhiteSpace(id)))
            .WithMessage("All assignee IDs must be valid non-empty strings.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Title) || 
                      !string.IsNullOrEmpty(x.Description) || 
                      x.DeadlineAt.HasValue || 
                      x.Priority.HasValue || 
                      x.Status.HasValue ||
                      x.AssigneeIds.Any())
            .WithMessage("At least one field must be provided for update.");
    }
}