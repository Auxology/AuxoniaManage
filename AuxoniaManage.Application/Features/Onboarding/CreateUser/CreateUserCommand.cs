using System.Windows.Input;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.CreateUser;

public sealed record CreateUserCommand
(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : ITransactionalCommand<CreateUserResponse>;

public sealed record CreateUserResponse
(
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt
);