namespace AuxoniaManage.Domain.Events.Membership;

public sealed record MembershipCreatedEvent
(
    string UserId,
    Guid WorkspaceId,
    DateTime CreatedAt
);