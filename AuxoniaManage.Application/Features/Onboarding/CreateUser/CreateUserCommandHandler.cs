using Ardalis.GuardClauses;
using AuxoniaManage.Application.Features.Auth.Register;
using AuxoniaManage.Application.Features.Profile.Create;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IMediator _mediator;
    
    public CreateUserCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));
        Guard.Against.NullOrEmpty(request.FirstName, nameof(request.FirstName));
        Guard.Against.NullOrEmpty(request.LastName, nameof(request.LastName));

        var fullName = $"{request.FirstName} {request.LastName}";
        
        var registerCommand = new RegisterCommand(request.Email, request.Password, fullName);
        
        var registerResponse = await _mediator.Send(registerCommand, cancellationToken);

        var createProfileCommand = new CreateProfileCommand
        (
            UserId: registerResponse.UserId,
            Email: registerResponse.Email,
            FirstName: request.FirstName,
            LastName: request.LastName
        );

        var createProfileResponse = await _mediator.Send(createProfileCommand, cancellationToken);

        return new CreateUserResponse
        (
            UserId: registerResponse.UserId,
            Email: registerResponse.Email,
            FirstName: createProfileResponse.FirstName,
            LastName: createProfileResponse.LastName,
            CreatedAt: DateTime.UtcNow
        );
    }
}