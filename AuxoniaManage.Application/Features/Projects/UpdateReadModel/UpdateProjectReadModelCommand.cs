using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.UpdateReadModel;

public sealed record UpdateProjectReadModelCommand
(
    Guid ProjectId,
    string Name,
    Guid WorkspaceId,
    string? LogoKey
) : ICommand<UpdateProjectReadModelResponse>;

public sealed record UpdateProjectReadModelResponse
(
    Guid Id,
    Guid ProjectId,
    string Name,
    Guid WorkspaceId,
    string? LogoKey,
    DateTime UpdatedAt
);