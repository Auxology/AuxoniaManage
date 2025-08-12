using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.Leave;

public sealed class LeaveWorkspaceCommandHandler : ICommandHandler<LeaveWorkspaceCommand, LeaveWorkspaceResponse>
{
    private readonly IMembershipRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public LeaveWorkspaceCommandHandler
    (
        IMembershipRepository repository,
        IPublishEndpoint publishEndpoint
    )
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<LeaveWorkspaceResponse> Handle(LeaveWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        
        var membership = await _repository.GetSpecificAsync(
            request.WorkspaceId,
            request.UserId,
            cancellationToken
        );

        if (membership == null)
        {
            throw new MembershipNotFoundException();
        }

        if (membership.Role == WorkspaceRoles.Owner)
        {
            throw new TransferOwnershipFirstException();
        }
        
        var isSuccess = await _repository.DeleteAsync(membership, cancellationToken);
        
        if (!isSuccess)
        {
            throw new MembershipDeletionFailedException();
        }
        
        var membershipDeletedEvent = new MembershipDeletedEvent
        (
            WorkspaceId: request.WorkspaceId,
            UserId: request.UserId,
            DeletedAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(membershipDeletedEvent, cancellationToken);
        
        return new LeaveWorkspaceResponse
        (
            WorkspaceId: request.WorkspaceId,
            UserId: request.UserId,
            LeftAt: DateTime.UtcNow
        );
    }
}