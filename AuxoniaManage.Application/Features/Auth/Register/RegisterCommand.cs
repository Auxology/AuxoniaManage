using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Auth.Register;

public record RegisterCommand
(
    string Email,
    string Password,
    string FullName
) : ICommand<RegisterResponse>;

public record RegisterResponse 
(
    string UserId,
    string Email,
    DateTime CreatedAt
);