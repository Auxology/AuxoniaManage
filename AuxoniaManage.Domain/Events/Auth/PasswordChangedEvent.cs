namespace AuxoniaManage.Domain.Events.Auth;

public sealed record PasswordChangedEvent
(
    string UserId,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime ChangedAt
);