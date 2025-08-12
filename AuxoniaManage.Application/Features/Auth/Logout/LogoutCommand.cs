using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Logout;

public sealed record LogoutCommand
(
    string UserId
) : ICommand<LogoutResponse>;
    
public sealed record LogoutResponse 
(
    string Id,
    DateTime LoggedOutAt
);