using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.UpdateReadModel;

public sealed record UpdateWorkspaceReadModelCommand
(
    Guid WorkspaceId,
    string Name,
    string? LogoKey
) : ICommand<UpdateWorkspaceReadModelResponse>;

public sealed record UpdateWorkspaceReadModelResponse
(
    Guid Id,
    string Name,
    string? LogoKey,
    DateTime UpdatedAt
);