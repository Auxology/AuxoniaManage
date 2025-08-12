using Ardalis.GuardClauses;
using AuxoniaManage.Application.Features.Membership.DeleteMemberships;
using AuxoniaManage.Application.Features.Projects.DeleteMany;
using AuxoniaManage.Application.Features.ProjectTask.DeleteMany;
using AuxoniaManage.Application.Features.Workspace.Delete;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.DeleteWorkspaceOnboarding;

public sealed class DeleteWorkspaceOnboardingCommandHandler : ICommandHandler<DeleteWorkspaceOnboardingCommand, DeleteWorkspaceOnboardingResponse>
{
    private readonly IMediator _mediator;

    public DeleteWorkspaceOnboardingCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<DeleteWorkspaceOnboardingResponse> Handle(DeleteWorkspaceOnboardingCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var deleteProjectsCommand = new DeleteProjectsCommand
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId
        );
        
        var deleteProjectsResponse = await _mediator.Send(deleteProjectsCommand, cancellationToken);
        
        var deleteProjectTasksCommand = new DeleteProjectTasksCommand
        (
            UserId: request.UserId,
            ProjectIds: deleteProjectsResponse.ProjectIds,
            WorkspaceId: request.WorkspaceId
        );
        
        var deleteProjectTasksResponse = await _mediator.Send(deleteProjectTasksCommand, cancellationToken);
        
        var deleteMembershipsCommand = new DeleteMembershipsCommand
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId
        );
        
        var deleteMembershipsResponse = await _mediator.Send(deleteMembershipsCommand, cancellationToken);
        
        var deleteWorkspaceCommand = new DeleteWorkspaceCommand
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId
        );
        
        var deleteWorkspaceResponse = await _mediator.Send(deleteWorkspaceCommand, cancellationToken);
        
        return new DeleteWorkspaceOnboardingResponse
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId,
            ProjectIds: deleteProjectsResponse.ProjectIds.ToList(),
            TaskIds: deleteProjectTasksResponse.DeletedTaskIds,
            DeletedAt: DateTime.UtcNow
        );
    }
}