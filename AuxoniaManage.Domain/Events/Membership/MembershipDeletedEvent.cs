namespace AuxoniaManage.Domain.Events.Membership;

public sealed record MembershipDeletedEvent
(
    Guid WorkspaceId,
    string UserId,
    DateTime DeletedAt
);