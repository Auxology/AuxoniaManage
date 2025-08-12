using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Membership.DeleteMemberships;

public sealed record DeleteMembershipsCommand
(
    string UserId,
    Guid WorkspaceId
) : ICommand<DeleteMembershipsResponse>;

public sealed record DeleteMembershipsResponse
(
    Guid WorkspaceId,
    string DeletedBy,
    DateTime DeletedAt
);