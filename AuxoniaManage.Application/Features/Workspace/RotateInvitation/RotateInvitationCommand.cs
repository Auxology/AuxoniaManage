using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.RotateInvitation;

public sealed record RotateInvitationCommand
(
    string UserId,
    Guid WorkspaceId
) : ICommand<RotateInvitationResponse>;

public sealed record RotateInvitationResponse
(
    Guid WorkspaceId,
    string RotatedById,
    string NewInvitationCode,
    DateTime RotatedAt
);