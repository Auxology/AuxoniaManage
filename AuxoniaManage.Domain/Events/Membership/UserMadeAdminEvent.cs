namespace AuxoniaManage.Domain.Events.Membership;

public sealed record UserMadeAdminEvent
(
    Guid WorkspaceId,
    string UserId,
    string NewAdminId,
    DateTime UpdatedAt
);