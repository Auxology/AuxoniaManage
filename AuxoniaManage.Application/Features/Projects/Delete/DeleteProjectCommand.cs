using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.Delete;

public sealed record DeleteProjectCommand
(
    string UserId,
    Guid WorkspaceId,
    Guid ProjectId
 
) : ICommand<DeleteProjectResponse>;

public sealed record DeleteProjectResponse
(
    string DeletedById,
    Guid WorkspaceId,
    Guid ProjectId,
    DateTime DeletedAt
);