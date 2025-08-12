using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.GetWorkspaces;

public sealed record GetWorkspacesQuery
(
    string UserId
) : IQuery<GetWorkspacesResponse>;

public sealed record WorkspaceDto
(
    Guid WorkspaceId,
    string Name,
    string? LogoUrl
);

public sealed record GetWorkspacesResponse
(
    IReadOnlyList<WorkspaceDto> Workspaces
);