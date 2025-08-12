using FluentValidation;

namespace AuxoniaManage.Application.Features.ProjectTask.Create;

internal sealed  class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Task title is required.")
            .MaximumLength(50)
            .WithMessage("Task title must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage("Task description must not exceed 100 characters.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future.");
        
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid priority value.");
        
        RuleFor(x => x.Status) 
            .IsInEnum()
            .WithMessage("Invalid status value.");
    }
}