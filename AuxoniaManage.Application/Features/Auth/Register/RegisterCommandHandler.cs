using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterCommandHandler
    (
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }


    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));
        Guard.Against.NullOrEmpty(request.FullName, nameof(request.FullName));
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user != null)
        {
            throw new UserAlreadyExistsException();
        }
        
        user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            throw new UserRegistrationFailedException();
        }
        
        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
  
        var userRegisteredEvent = new UserRegisteredEvent
        (
            Id: user.Id,
            Email: user.Email,
            FullName: request.FullName,
            VerificationToken: verificationToken,
            CreatedAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(userRegisteredEvent, cancellationToken);
        
        return new RegisterResponse
        (
            UserId: user.Id,
            Email: user.Email,
            CreatedAt: DateTime.UtcNow
        );
    }
}