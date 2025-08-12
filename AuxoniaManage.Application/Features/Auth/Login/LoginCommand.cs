using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Login;

public sealed record LoginCommand
(
    string Email,
    string Password,
    string IpAddress,
    string UserAgent,
    bool RememberMe = false
) : ICommand<LoginResponse>;
    
public sealed record LoginResponse 
(
    string UserId,
    DateTime LoggedInAt
);