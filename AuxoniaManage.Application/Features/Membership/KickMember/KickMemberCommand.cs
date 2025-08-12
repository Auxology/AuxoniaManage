using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.KickMember;

public sealed record KickMemberCommand
(
    Guid WorkspaceId,
    string UserId,
    string MemberId
) : ICommand<KickMemberResponse>;

public sealed record KickMemberResponse
(
    Guid WorkspaceId,
    string KickedMemberId,
    string KickedByUserId,
    DateTime KickedAt
);