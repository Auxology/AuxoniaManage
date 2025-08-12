using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.CreateReadModel;

public sealed record CreateWorkspaceReadModelCommand(
    Guid WorkspaceId,
    string Name,
    string? LogoKey
) : ITransactionalCommand<CreateWorkspaceReadModelResponse>;

public sealed record CreateWorkspaceReadModelResponse
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? LogoKey,
    DateTime CreatedAt
);