using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.Get;

public sealed record GetWorkspaceQuery
(
    string UserId,
    Guid WorkspaceId
) : IQuery<GetWorkspaceResponse>;

public sealed record GetWorkspaceResponse
(
    Guid Id,
    string Name,
    string Description,
    string OwnerId,
    string InvitationToken,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? LogoUrl
);