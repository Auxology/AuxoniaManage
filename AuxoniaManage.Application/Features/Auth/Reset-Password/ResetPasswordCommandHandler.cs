using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AuxoniaManage.Application.Features.Auth.Reset_Password;

public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public ResetPasswordCommandHandler
    (
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Token, nameof(request.Token));
        Guard.Against.NullOrEmpty(request.NewPassword, nameof(request.NewPassword));
        
        var user = await _userManager.FindByIdAsync(request.UserId);
        
        if (user == null)
        {
            throw new PasswordResetFailedException();
        }
        
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        
        if (!result.Succeeded)
        {
            throw new PasswordResetFailedException();
        }
        
        await _userManager.UpdateSecurityStampAsync(user);
        
        var passwordResetEvent = new PasswordResetEvent
        (
            Id: user.Id,
            Email: user.Email!,
            ResetAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(passwordResetEvent, cancellationToken);
        
        return new ResetPasswordResponse
        (
            Email: user.Email!,
            ResetAt: DateTime.UtcNow
        );
    }
}