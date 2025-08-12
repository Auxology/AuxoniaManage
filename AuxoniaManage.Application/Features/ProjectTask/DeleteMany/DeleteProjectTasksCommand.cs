using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.ProjectTask.DeleteMany;

public sealed record DeleteProjectTasksCommand
(
    string UserId,
    IReadOnlyCollection<Guid> ProjectIds,
    Guid WorkspaceId
) : ICommand<DeleteProjectTasksResponse>;

public sealed record DeleteProjectTasksResponse
(
    IReadOnlyList<Guid> DeletedTaskIds,
    Guid WorkspaceId,
    IReadOnlyCollection<Guid> ProjectIds,
    string DeletedById,
    DateTime DeletedAt
);