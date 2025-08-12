using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Projects.Update;

public sealed record UpdateProjectCommand
(
    Guid Id,
    Guid WorkspaceId,
    string UserId,
    string? Name,
    IFormFile? Logo
) : ITransactionalCommand<UpdateProjectResponse>;

public sealed record UpdateProjectResponse
(
    Guid Id,
    string Name,
    string? LogoUrl,
    DateTime UpdatedAt,
    string UpdatedById
);