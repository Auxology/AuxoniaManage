namespace AuxoniaManage.Domain.Events.Auth;

public sealed record EmailVerifiedEvent
(
    string Id,
    string Email,
    DateTime VerifiedAt
);