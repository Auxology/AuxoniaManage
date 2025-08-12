using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Membership;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Membership.MakeAdmin;

public sealed class MakeAdminCommandHandler : ICommandHandler<MakeAdminCommand, MakeAdminResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public MakeAdminCommandHandler
    (
        IMembershipRepository membershipRepository,
        IPublishEndpoint publishEndpoint
    )
    

    {
        _membershipRepository = membershipRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<MakeAdminResponse> Handle(MakeAdminCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.NewAdminId, nameof(request.NewAdminId));
        
        var userMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.UserId, cancellationToken);

        if (userMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.UserId);
        }

        if (userMembership.Role != WorkspaceRoles.Owner)
        {
            throw new OnlyOwnerCanKickMemberException();
        }
        
        var newAdminMembership = await _membershipRepository.GetSpecificAsync(request.WorkspaceId, request.NewAdminId, cancellationToken);
        
        if (newAdminMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.NewAdminId);
        }
        
        if (newAdminMembership.Role == WorkspaceRoles.Admin)
        {
            throw new MembershipAlreadyExistsException(request.WorkspaceId, request.NewAdminId);
        }
        
        var timeStamp = DateTime.UtcNow;
        
        newAdminMembership.UpdateMembership
        (
            role: WorkspaceRoles.Admin,
            timeStamp: timeStamp
        );
        
        var isSuccess = await _membershipRepository.UpdateAsync(newAdminMembership, cancellationToken);

        if (!isSuccess)
        {
            throw new MembershipUpdateFailedException();
        }
        
        var userMadeAdminEvent = new UserMadeAdminEvent
        (
            WorkspaceId: request.WorkspaceId,
            UserId: userMembership.UserId,
            NewAdminId: newAdminMembership.UserId,
            UpdatedAt: timeStamp
        );
        
        await _publishEndpoint.Publish(userMadeAdminEvent, cancellationToken);
        
        return new MakeAdminResponse
        (
            WorkspaceId: request.WorkspaceId,
            UserId: userMembership.UserId,
            NewAdminId: newAdminMembership.UserId,
            UpdatedAt: timeStamp
        );
    }
}