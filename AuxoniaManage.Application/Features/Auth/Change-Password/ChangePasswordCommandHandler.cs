using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AuxoniaManage.Application.Features.Auth.Change_Password;

public sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public ChangePasswordCommandHandler
    (
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.OldPassword, nameof(request.OldPassword));
        Guard.Against.NullOrEmpty(request.NewPassword, nameof(request.NewPassword));
        Guard.Against.NullOrEmpty(request.IpAddress, nameof(request.IpAddress));
        Guard.Against.NullOrEmpty(request.UserAgent, nameof(request.UserAgent));
        
        if (request.OldPassword == request.NewPassword)
        {
            throw new OldPasswordCannotBeSameAsNewPasswordException();
        }
        
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        
        if (!result.Succeeded)
        {
            throw new FailedToChangePasswordException();
        }
        
        await _userManager.UpdateSecurityStampAsync(user);

        var passwordChangedEvent = new PasswordChangedEvent
        (
            UserId: user.Id,
            Email: user.Email!,
            IpAddress: request.IpAddress,
            UserAgent: request.UserAgent,
            ChangedAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(passwordChangedEvent, cancellationToken);
        
        return new ChangePasswordResponse
        (
            UserId: user.Id,
            ChangedAt: DateTime.UtcNow
        );
    }
}