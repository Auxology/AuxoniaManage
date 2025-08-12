using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Profile.Get;

public sealed record GetProfileQuery
(
    string UserId
) : IQuery<GetProfileResponse>;

public sealed record GetProfileResponse
(
    Guid Id,
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? AvatarUrl
);