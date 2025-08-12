namespace AuxoniaManage.Presentation.Dto.Notification;

public sealed record ArchiveNotificationsRequest
(
    IReadOnlyCollection<Guid> NotificationIds
);