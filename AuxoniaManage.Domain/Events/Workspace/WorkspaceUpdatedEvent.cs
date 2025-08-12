namespace AuxoniaManage.Domain.Events.Workspace;

public record WorkspaceUpdatedEvent
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? LogoKey
);