namespace AuxoniaManage.Domain.Events.Profile;

public record ProfileUpdatedEvent
(
    Guid Id,
    string UserId,
    string FullName,
    string? AvatarKey
);