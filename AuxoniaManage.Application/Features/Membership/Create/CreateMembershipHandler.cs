using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.Create;

public sealed class CreateMembershipHandler : ICommandHandler<CreateMembershipCommand, CreateMembershipResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateMembershipHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<CreateMembershipResponse> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.EnumOutOfRange(request.Role, nameof(request.Role));
        
        var existingMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.UserId, cancellationToken);
        
        if (existingMembership != null)
        {
            throw new MembershipAlreadyExistsException(request.WorkspaceId, request.UserId);
        }

        var newMembership = new Domain.Entities.Membership(
            userId: request.UserId,
            workspaceId: request.WorkspaceId,
            role: request.Role,
            timeStamp: DateTime.UtcNow
        );
        
        var isSuccess = await _membershipRepository.AddAsync(newMembership, cancellationToken);
        
        if (!isSuccess)
        {
            throw new MembershipCreationFailedException(request.WorkspaceId, request.UserId);
        }

        var membershipCreatedEvent = new MembershipCreatedEvent
        (
            UserId: newMembership.UserId,
            WorkspaceId: newMembership.WorkspaceId,
            CreatedAt: newMembership.JoinedAt
        );
        
        await _publishEndpoint.Publish(membershipCreatedEvent, cancellationToken);
        
        return new CreateMembershipResponse
        (
            Id: newMembership.Id,
            UserId: newMembership.UserId,
            WorkspaceId: newMembership.WorkspaceId,
            Role: newMembership.Role,
            JoinedAt: newMembership.JoinedAt
        );
    }
}