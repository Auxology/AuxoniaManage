using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Reset_Password;

public sealed record ResetPasswordCommand
(
    string UserId,
    string Token,
    string NewPassword
) : ICommand<ResetPasswordResponse>;

public sealed record ResetPasswordResponse 
(
    string Email,
    DateTime ResetAt
);