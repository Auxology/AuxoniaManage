using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.TransferOwnership;

public sealed record TransferOwnershipCommand
(
    string UserId,
    string NewOwnerId,
    Guid WorkspaceId
) : ICommand<TransferOwnershipResponse>;

public sealed record TransferOwnershipResponse
(
    string UserId,
    string NewOwnerId,
    Guid WorkspaceId,
    DateTime TransferDate
);