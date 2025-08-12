using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.DeleteMany;

public sealed record DeleteProjectsCommand
(
    string UserId,
    Guid WorkspaceId
) : ICommand<DeleteProjectsResponse>;

public sealed record DeleteProjectsResponse 
(
    string UserId,
    Guid WorkspaceId,
    IReadOnlyCollection<Guid> ProjectIds,
    DateTime DeletedAt
);