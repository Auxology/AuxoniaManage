using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.MakeMember;

public sealed class MakeMemberCommandHandler : ICommandHandler<MakeMemberCommand, MakeMemberResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public MakeMemberCommandHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    

    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<MakeMemberResponse> Handle(MakeMemberCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.NewMemberId, nameof(request.NewMemberId));
        
        var userMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.UserId);
        }

        if (userMembership.Role != WorkspaceRoles.Owner)
        {
            throw new OnlyOwnerCanKickMemberException();
        }
        
        var newMemberMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.NewMemberId, cancellationToken);
        
        if (newMemberMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.NewMemberId);
        }
        
        if (newMemberMembership.Role == WorkspaceRoles.Member)
        {
            throw new MembershipAlreadyExistsException(request.WorkspaceId, request.NewMemberId);
        }
        
        var timeStamp = DateTime.UtcNow;
        
        newMemberMembership.UpdateMembership
        (
            role: WorkspaceRoles.Member,
            timeStamp: timeStamp
        );
        
        var isSuccess = await _membershipRepository.UpdateAsync(newMemberMembership, cancellationToken);

        if (!isSuccess)
        {
            throw new MembershipUpdateFailedException();
        }
        
        var userMadeMemberEvent = new UserMadeMemberEvent
        (
            WorkspaceId: request.WorkspaceId,
            UserId: userMembership.UserId,
            NewMemberId: newMemberMembership.UserId,
            UpdatedAt: timeStamp
        );
        
        await _publishEndpoint.Publish(userMadeMemberEvent, cancellationToken);
        
        return new MakeMemberResponse
        (
            WorkspaceId: request.WorkspaceId,
            UserId: userMembership.UserId,
            NewMemberId: newMemberMembership.UserId,
            UpdatedAt: timeStamp
        );
    }
}