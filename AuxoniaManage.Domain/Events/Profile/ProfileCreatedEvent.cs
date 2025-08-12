namespace AuxoniaManage.Domain.Events.Profile;

public sealed record ProfileCreatedEvent
(
    Guid Id,
    string UserId,
    string FullName,
    string Email
);