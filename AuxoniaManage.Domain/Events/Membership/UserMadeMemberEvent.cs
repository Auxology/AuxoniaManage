namespace AuxoniaManage.Domain.Events.Membership;

public sealed record UserMadeMemberEvent
(
    Guid WorkspaceId,
    string UserId,
    string NewMemberId,
    DateTime UpdatedAt
);