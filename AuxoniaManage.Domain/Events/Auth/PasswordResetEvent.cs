namespace AuxoniaManage.Domain.Events.Auth;

public sealed record PasswordResetEvent
(
    string Id,
    string Email,
    DateTime ResetAt
);