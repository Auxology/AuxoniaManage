using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Forgot_Password;

public sealed record ForgotPasswordCommand
(
    string Email
) : ICommand<ForgotPasswordResponse>;

public sealed record ForgotPasswordResponse 
(
    string Email,
    DateTime RequestedAt
);