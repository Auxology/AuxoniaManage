using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.DeleteMemberships;

public sealed class DeleteMembershipsCommandHandler : ICommandHandler<DeleteMembershipsCommand, DeleteMembershipsResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteMembershipsCommandHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<DeleteMembershipsResponse> Handle(DeleteMembershipsCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var userMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userMembership == null || userMembership.Role != WorkspaceRoles.Owner)
        {
            throw new NotEnoughPermissionsException();
        }
        
        var allMemberships = await _membershipRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        if (allMemberships.Count == 0)
        {
            throw new MembershipNotFoundException();
        }
        
        var isSuccess = await _membershipRepository.DeleteRangeAsync(allMemberships, cancellationToken);
        
        if (!isSuccess)
        {
            throw new MembershipDeletionFailedException();
        }
        
        var timeStamp = DateTime.UtcNow;
        
        var membershipsDeletedEvent = new MembershipsDeletedEvent
        (
            request.WorkspaceId,
            request.UserId,
            timeStamp
        );
        
        await _publishEndpoint.Publish(membershipsDeletedEvent, cancellationToken);
        
        return new DeleteMembershipsResponse
        (
            request.WorkspaceId,
            request.UserId,
            timeStamp
        );
    }
}