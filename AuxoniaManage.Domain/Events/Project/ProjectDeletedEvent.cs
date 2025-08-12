namespace AuxoniaManage.Domain.Events.Project;

public sealed record ProjectDeletedEvent
(
    Guid Id,
    Guid WorkspaceId
);