using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.CreateReadModel;

public sealed record CreateProjectReadModelCommand
(
    Guid ProjectId,
    string Name,
    Guid WorkspaceId,
    string? LogoKey
) : ICommand<CreateProjectReadModelResponse>;

public sealed record CreateProjectReadModelResponse
(
    Guid Id,
    Guid ProjectId,
    string Name,
    Guid WorkspaceId,
    string? LogoKey,
    DateTime CreatedAt
);