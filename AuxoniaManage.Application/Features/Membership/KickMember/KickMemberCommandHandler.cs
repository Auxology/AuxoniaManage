using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.KickMember;

public sealed class KickMemberCommandHandler : ICommandHandler<KickMemberCommand, KickMemberResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public KickMemberCommandHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<KickMemberResponse> Handle(KickMemberCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.MemberId, nameof(request.MemberId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var userMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.UserId);
        }

        if (userMembership.Role != WorkspaceRoles.Owner)
        {
            throw new OnlyOwnerCanKickMemberException();
        }
        
        var memberToKick = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.MemberId, cancellationToken);
        
        if (memberToKick == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.MemberId);
        }
        
        var isSuccess = await _membershipRepository.DeleteAsync(memberToKick, cancellationToken);
        
        if (!isSuccess)
        {
            throw new FailedToKickMemberException();
        }
        
        var timeStamp = DateTime.UtcNow;
        
        var memberKickedEvent = new MemberKickedEvent
        (
            WorkspaceId: request.WorkspaceId,
            KickedMemberId: memberToKick.UserId,
            KickedByUserId: userMembership.UserId,
            KickedAt: timeStamp
        );
        
        await _publishEndpoint.Publish(memberKickedEvent, cancellationToken);
        
        return new KickMemberResponse
        (
            WorkspaceId: request.WorkspaceId,
            KickedMemberId: memberToKick.UserId,
            KickedByUserId: userMembership.UserId,
            KickedAt: timeStamp
        );
    }
}