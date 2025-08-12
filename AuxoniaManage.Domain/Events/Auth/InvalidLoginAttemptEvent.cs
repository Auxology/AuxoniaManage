namespace AuxoniaManage.Domain.Events.Auth;

public sealed record InvalidLoginAttemptEvent
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime Timestamp,
    int FailedAttempts
);