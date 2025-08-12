using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.AcceptInvitation;

public sealed record AcceptInvitationCommand
(
    string UserId,
    Guid WorkspaceId,
    string InvitationToken
) : ICommand<AcceptInvitationResponse>;

public sealed record AcceptInvitationResponse
(
    string UserId,
    Guid WorkspaceId,
    string InvitationToken,
    DateTime AcceptedAt
);