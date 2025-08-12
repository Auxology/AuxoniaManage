namespace AuxoniaManage.Domain.Events.Membership;

public sealed record OwnershipTransferredEvent
(
    string PreviousOwnerId,
    string NewOwnerId,
    Guid WorkspaceId,
    DateTime TransferredAt
);