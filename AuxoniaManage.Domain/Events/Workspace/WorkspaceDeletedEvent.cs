namespace AuxoniaManage.Domain.Events.Workspace;

public record WorkspaceDeletedEvent
(
    Guid WorkspaceId,
    string Name
);
