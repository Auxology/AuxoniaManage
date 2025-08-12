namespace AuxoniaManage.Domain.Events.Project;

public sealed record ProjectCreatedEvent
(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? LogoKey
);