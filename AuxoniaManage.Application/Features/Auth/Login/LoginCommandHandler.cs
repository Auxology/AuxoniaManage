using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AuxoniaManage.Application.Features.Auth.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public LoginCommandHandler
    (
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));
        Guard.Against.NullOrEmpty(request.IpAddress, nameof(request.IpAddress));
        Guard.Against.NullOrEmpty(request.UserAgent, nameof(request.UserAgent));
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        
        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isCorrectPassword)
        {
            await _userManager.AccessFailedAsync(user);

            var failedLoginEvent = new InvalidLoginAttemptEvent
            (
                Id: user.Id,
                Email: user.Email!,
                IpAddress: request.IpAddress,
                UserAgent: request.UserAgent,
                FailedAttempts: await _userManager.GetAccessFailedCountAsync(user),
                Timestamp: DateTime.UtcNow
            );
            
            await _publishEndpoint.Publish(failedLoginEvent, cancellationToken);
            
            throw new InvalidCredentialsException();
        }
        
        var isLockedOut = await _userManager.IsLockedOutAsync(user);
        
        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        
        if (isLockedOut)
        {
            var lockoutTriggeredEvent = new LockoutTriggeredEvent
            (
                Id: user.Id,
                Email: user.Email!,
                IpAddress: request.IpAddress,
                UserAgent: request.UserAgent,
                LockedAt: DateTime.UtcNow
            );
            
            await _publishEndpoint.Publish(lockoutTriggeredEvent, cancellationToken);
            
            throw new UserLockedOutException();
        }
        
        if (!isConfirmed)
        {
            throw new EmailNotConfirmedException();
        }
        
        await _userManager.ResetAccessFailedCountAsync(user);
        
        await _signInManager.SignInAsync(user, request.RememberMe);
        
        var successfulLoginEvent = new SuccessfulLoginEvent
        (
            Id: user.Id,
            Email: user.Email!,
            IpAddress: request.IpAddress,
            UserAgent: request.UserAgent,
            LoginTime: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(successfulLoginEvent, cancellationToken);

        return new LoginResponse
        (
            UserId: user.Id,
            LoggedInAt: DateTime.UtcNow
        );
    }
}