using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Projects.Create;

public sealed record CreateProjectCommand
(
    string Name,
    Guid WorkspaceId,
    string UserId,
    IFormFile? Logo
) : ICommand<CreateProjectResponse>;

public sealed record CreateProjectResponse
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? LogoKey,
    DateTime CreatedAt
);