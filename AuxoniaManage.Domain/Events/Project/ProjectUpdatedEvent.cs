namespace AuxoniaManage.Domain.Events.Project;

public sealed record ProjectUpdatedEvent
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? LogoKey
);