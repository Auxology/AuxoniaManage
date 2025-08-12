using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.Get;

public sealed record GetProjectQuery
(
    string UserId,
    Guid WorkspaceId,
    Guid Id
) : IQuery<GetProjectResponse>;

public sealed record GetProjectResponse
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    DateTime CreatedAt,
    string? LogoUrl
);