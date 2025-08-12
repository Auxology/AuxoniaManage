using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.TransferOwnership;

public sealed class TransferOwnershipCommandHandler : ICommandHandler<TransferOwnershipCommand, TransferOwnershipResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public TransferOwnershipCommandHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<TransferOwnershipResponse> Handle(TransferOwnershipCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.NewOwnerId, nameof(request.NewOwnerId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));

        var oldOwnersMembership = await _membershipRepository.GetSpecificAsync(
            request.WorkspaceId,
            request.UserId,
            cancellationToken
        );
        
        if (oldOwnersMembership == null)
        {
            throw new MembershipNotFoundException();
        }
        
        if (oldOwnersMembership.Role != WorkspaceRoles.Owner)
        {
            throw new OnlyOwnerCanTransferOwnershipException();
        }
        
        var newOwnersMembership = await _membershipRepository.GetSpecificAsync(
            request.WorkspaceId,
            request.NewOwnerId,
            cancellationToken
        );
        
        if (newOwnersMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.NewOwnerId);
        }
        
        oldOwnersMembership.UpdateMembership(WorkspaceRoles.Member, DateTime.UtcNow);
        
        newOwnersMembership.UpdateMembership(WorkspaceRoles.Owner, DateTime.UtcNow);
        
        var oldOwnerUpdateResult = await _membershipRepository.UpdateAsync(oldOwnersMembership, cancellationToken);
        
        if (!oldOwnerUpdateResult)
        {
            throw new MembershipUpdateFailedException();
        }
        
        var newOwnerUpdateResult = await _membershipRepository.UpdateAsync(newOwnersMembership, cancellationToken);
        
        if (!newOwnerUpdateResult)
        {
            throw new MembershipUpdateFailedException();
        }
        
        var ownershipTransferEvent = new OwnershipTransferredEvent
        (
            oldOwnersMembership.UserId,
            newOwnersMembership.UserId,
            newOwnersMembership.WorkspaceId,
            DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(ownershipTransferEvent, cancellationToken);
        
        return new TransferOwnershipResponse
        (
            UserId: oldOwnersMembership.UserId,
            NewOwnerId: newOwnersMembership.UserId,
            WorkspaceId: oldOwnersMembership.WorkspaceId,
            TransferDate: DateTime.UtcNow
        );
    }
}