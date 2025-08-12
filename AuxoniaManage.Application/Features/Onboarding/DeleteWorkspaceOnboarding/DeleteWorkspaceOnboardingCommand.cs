using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.DeleteWorkspaceOnboarding;

public sealed record DeleteWorkspaceOnboardingCommand
(
    string UserId,
    Guid WorkspaceId
) : ITransactionalCommand<DeleteWorkspaceOnboardingResponse>;

public sealed record DeleteWorkspaceOnboardingResponse
(
    string UserId,
    Guid WorkspaceId,
    IReadOnlyList<Guid> ProjectIds,
    IReadOnlyList<Guid> TaskIds,
    DateTime DeletedAt
);