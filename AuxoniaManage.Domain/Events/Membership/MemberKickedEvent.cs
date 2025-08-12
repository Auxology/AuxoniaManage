namespace AuxoniaManage.Domain.Events.Membership;

public sealed record MemberKickedEvent
(
    Guid WorkspaceId,
    string KickedMemberId,
    string KickedByUserId,
    DateTime KickedAt
);