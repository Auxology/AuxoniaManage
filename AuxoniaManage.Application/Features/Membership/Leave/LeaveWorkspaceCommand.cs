using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.Leave;

public sealed record LeaveWorkspaceCommand
(
    Guid WorkspaceId,
    string UserId
) : ICommand<LeaveWorkspaceResponse>;

public sealed record LeaveWorkspaceResponse
(
    Guid WorkspaceId,
    string UserId,
    DateTime LeftAt
);