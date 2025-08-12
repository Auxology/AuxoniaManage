using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.GetWorkspaceMemberships;

public sealed record GetWorkspaceMembershipsQuery
(
    Guid WorkspaceId,
    string UserId
) : IQuery<GetWorkspaceMembershipsResponse>;

public sealed record GetWorkspaceMembershipsResponse
(
    IReadOnlyList<MembershipDto> Memberships
);

public sealed record MembershipDto
(
    Guid Id,
    string UserId,
    string UserName,
    string? AvatarUrl,
    Guid WorkspaceId,
    string Role,
    DateTime JoinedAt,
    DateTime UpdatedAt
);