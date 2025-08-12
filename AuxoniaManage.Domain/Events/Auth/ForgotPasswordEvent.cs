namespace AuxoniaManage.Domain.Events.Auth;

public sealed record ForgotPasswordEvent
(
    string Id,
    string Email,
    string ResetToken,
    DateTime RequestedAt
);