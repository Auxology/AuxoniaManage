using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.MakeAdmin;

public sealed record MakeAdminCommand
(
    Guid WorkspaceId,
    string UserId,
    string NewAdminId
) : ICommand<MakeAdminResponse>;

public sealed record MakeAdminResponse
(
    Guid WorkspaceId,
    string UserId,
    string NewAdminId,
    DateTime UpdatedAt
);