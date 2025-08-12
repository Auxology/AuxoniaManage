namespace AuxoniaManage.Domain.Events.Auth;

public sealed record SuccessfulLoginEvent
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime LoginTime
);