using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;

namespace AuxoniaManage.Application.Features.Profile.Update;

public sealed record UpdateProfileCommand
(
    string UserId,
    string? FirstName,
    string? LastName,
    IFormFile? Avatar
) : ITransactionalCommand<UpdateProfileResponse>;

public sealed record UpdateProfileResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime UpdatedAt,
    string? AvatarKey
);