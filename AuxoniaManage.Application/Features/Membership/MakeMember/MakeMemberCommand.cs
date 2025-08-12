using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.MakeMember;

public sealed record MakeMemberCommand
(
    Guid WorkspaceId,
    string UserId,
    string NewMemberId
) : ICommand<MakeMemberResponse>;

public sealed record MakeMemberResponse
(
    Guid WorkspaceId,
    string UserId,
    string NewMemberId,
    DateTime UpdatedAt
);