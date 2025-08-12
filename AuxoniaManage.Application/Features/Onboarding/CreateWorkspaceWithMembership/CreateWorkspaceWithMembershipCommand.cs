using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Onboarding.CreateWorkspaceWithMembership;

public sealed record CreateWorkspaceWithMembershipCommand
(
    string Name,
    string Description,
    string UserId,
    IFormFile? Logo = null
) : ITransactionalCommand<CreateWorkspaceWithMembershipResponse>;

public sealed record CreateWorkspaceWithMembershipResponse
(
    Guid WorkspaceId,
    string OwnerId,
    Guid MembershipId,
    string Name,
    string Description,
    DateTime CreatedAt,
    string? LogoKey = null
);