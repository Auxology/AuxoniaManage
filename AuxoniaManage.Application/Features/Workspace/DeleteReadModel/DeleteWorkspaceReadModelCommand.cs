using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.DeleteReadModel;

public sealed record DeleteWorkspaceReadModelCommand
(
    Guid WorkspaceId
) : ICommand<DeleteWorkspaceReadModelResponse>;

public sealed record DeleteWorkspaceReadModelResponse
(
    bool IsDeleted,
    Guid WorkspaceId
);