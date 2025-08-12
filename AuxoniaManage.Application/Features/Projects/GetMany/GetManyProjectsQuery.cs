using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.GetMany;

public sealed record GetManyProjectsQuery
(
    string UserId,
    Guid WorkspaceId
) : IQuery<GetManyQueryResponse>;

public sealed record ProjectDto
(
    Guid Id,
    string Name,
    string? LogoUrl,
    Guid WorkspaceId
);

public sealed record GetManyQueryResponse
(
    IReadOnlyList<ProjectDto> Projects
);