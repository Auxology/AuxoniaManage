namespace AuxoniaManage.Domain.Events.Auth;

public sealed record LockoutTriggeredEvent
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime LockedAt
);