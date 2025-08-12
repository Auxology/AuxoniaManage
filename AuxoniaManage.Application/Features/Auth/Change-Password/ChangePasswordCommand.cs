using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Change_Password;

public sealed record ChangePasswordCommand
(
    string UserId,
    string OldPassword,
    string NewPassword,
    string IpAddress,
    string UserAgent
) : ICommand<ChangePasswordResponse>;

public sealed record ChangePasswordResponse
(
    string UserId,
    DateTime ChangedAt
);