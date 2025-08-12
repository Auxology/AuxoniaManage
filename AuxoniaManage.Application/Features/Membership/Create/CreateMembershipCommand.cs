using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.Create;

public sealed record CreateMembershipCommand
(
    string UserId,
    Guid WorkspaceId,
    WorkspaceRoles Role
) : ICommand<CreateMembershipResponse>;

public sealed record CreateMembershipResponse
(
    Guid Id,
    string UserId,
    Guid WorkspaceId,
    WorkspaceRoles Role,
    DateTime JoinedAt
);