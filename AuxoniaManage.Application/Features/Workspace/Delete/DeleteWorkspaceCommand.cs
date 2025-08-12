using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.Delete;

public sealed record DeleteWorkspaceCommand
(
    string UserId,
    Guid WorkspaceId
) : ICommand<DeleteWorkspaceResponse>;

public sealed record DeleteWorkspaceResponse
(
    Guid WorkspaceId,
    string DeletedBy,
    DateTime DeletedAt
);