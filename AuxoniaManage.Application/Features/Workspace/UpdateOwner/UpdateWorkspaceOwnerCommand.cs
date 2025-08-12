using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.UpdateOwner;

public sealed record UpdateWorkspaceOwnerCommand
(
    string NewOwnerId,
    Guid WorkspaceId
) : ICommand<UpdateWorkspaceOwnerResponse>;

public sealed record UpdateWorkspaceOwnerResponse
(
    string OwnerId,
    Guid WorkspaceId,
    DateTime UpdatedAt
);