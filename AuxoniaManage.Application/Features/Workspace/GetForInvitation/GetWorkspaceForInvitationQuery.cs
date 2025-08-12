using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.GetForInvitation;

public sealed record GetWorkspaceForInvitationQuery
(
    Guid WorkspaceId,
    string InvitationToken
) : IQuery<GetWorkspaceForInvitationResponse>;

public sealed record GetWorkspaceForInvitationResponse
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