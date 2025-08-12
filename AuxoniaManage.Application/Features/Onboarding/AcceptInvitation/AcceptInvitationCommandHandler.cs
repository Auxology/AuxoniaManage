using Ardalis.GuardClauses;
using AuxoniaManage.Application.Features.Membership.Create;
using AuxoniaManage.Application.Features.Workspace.GetForInvitation;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.AcceptInvitation;

public sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand, AcceptInvitationResponse>
{
    private readonly IMediator _mediator;

    public AcceptInvitationCommandHandler
    (
        IMediator mediator
    )
    
    {
        _mediator = mediator;
    }
    
    public async Task<AcceptInvitationResponse> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.InvitationToken, nameof(request.InvitationToken));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));

        var getWorkspaceQuery = new GetWorkspaceForInvitationQuery(WorkspaceId: request.WorkspaceId,
            InvitationToken: request.InvitationToken);
        
        var workspaceResponse = await _mediator.Send(getWorkspaceQuery, cancellationToken);

        var createMembershipCommand = new CreateMembershipCommand
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId,
            Role: WorkspaceRoles.Member
        );
        
        var membershipResponse = await _mediator.Send(createMembershipCommand, cancellationToken);
        
        
        return new AcceptInvitationResponse
        (
            UserId: request.UserId,
            WorkspaceId: request.WorkspaceId,
            InvitationToken: request.InvitationToken,
            AcceptedAt: DateTime.UtcNow
        );
    }
}