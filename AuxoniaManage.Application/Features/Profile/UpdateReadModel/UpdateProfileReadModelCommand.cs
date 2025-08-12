using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Profile.UpdateReadModel;

public sealed record UpdateProfileReadModelCommand
(
    Guid ProfileId,
    string UserId,
    string FullName,
    string? AvatarKey
) : ICommand<UpdateProfileReadModelResponse>;

public sealed record UpdateProfileReadModelResponse
(
    Guid Id,
    string FullName,
    string? AvatarKey,
    DateTime UpdatedAt
);