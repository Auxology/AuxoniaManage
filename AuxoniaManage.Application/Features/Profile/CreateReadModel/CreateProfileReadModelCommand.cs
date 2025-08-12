using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Profile.CreateReadModel;

public sealed record CreateProfileReadModelCommand
(
    Guid ProfileId,
    string UserId,
    string FullName,
    string Email
) : ICommand<CreateProfileReadModelResponse>;

public sealed record CreateProfileReadModelResponse
(
    Guid Id,
    string FullName,
    string Email,
    string? AvatarKey,
    DateTime CreatedAt
);