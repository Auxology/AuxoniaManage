using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Workspace.Update;

public sealed record UpdateWorkspaceCommand
(
    string UserId,
    Guid WorkspaceId,
    string? Name,
    string? Description,
    IFormFile? Logo
) : ITransactionalCommand<UpdateWorkspaceResponse>;

public sealed record UpdateWorkspaceResponse
(
    Guid Id,
    string Name,
    string Description,
    DateTime UpdatedAt,
    string? LogoKey
);