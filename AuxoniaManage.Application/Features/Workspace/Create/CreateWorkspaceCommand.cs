using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Workspace.Create;

public record CreateWorkspaceCommand
(
    string Name,
    string Description,
    string OwnerId,
    IFormFile? Logo
) : ITransactionalCommand<CreateWorkspaceResponse>;

public record CreateWorkspaceResponse
(
    Guid Id,
    string OwnerId,
    string Name,
    string Description,
    DateTime CreatedAt,
    string? LogoKey = null
);