using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Profile.Create;

public record CreateProfileCommand
(
    string UserId,
    string Email,
    string FirstName,
    string LastName
) : ICommand<CreateProfileResponse>;
        
public record CreateProfileResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime CreatedAt
);