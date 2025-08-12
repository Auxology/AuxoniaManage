namespace AuxoniaManage.Domain.Events.Auth;

public sealed record UserRegisteredEvent
(
    string Id,
    string Email,
    string FullName,
    string VerificationToken,
    DateTime CreatedAt
);