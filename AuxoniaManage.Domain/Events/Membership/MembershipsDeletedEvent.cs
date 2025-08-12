namespace AuxoniaManage.Domain.Events.Membership;

public record MembershipsDeletedEvent
(
    Guid WorkspaceId,
    string DeletedBy,
    DateTime DeletedAt
);