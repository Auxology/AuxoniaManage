namespace AuxoniaManage.Presentation.Dto.Notification;

public record ReadNotificationsRequest
(
    IReadOnlyCollection<Guid> NotificationIds
);