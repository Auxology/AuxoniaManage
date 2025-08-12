namespace AuxoniaManage.Domain.Events.Project;

public sealed record ProjectsDeletedEvent
(
    IReadOnlyCollection<Guid> Ids,
    Guid WorkspaceId
);