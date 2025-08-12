using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AuxoniaManage.Application.Features.Auth.Forgot_Password;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public ForgotPasswordCommandHandler
    (
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return new ForgotPasswordResponse
            (
                Email: request.Email,
                RequestedAt: DateTime.UtcNow
            );
        }
        
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var forgotPasswordEvent = new ForgotPasswordEvent
        (
            Id: user.Id,
            Email: user.Email!,
            ResetToken: resetToken,
            RequestedAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(forgotPasswordEvent, cancellationToken);
        
        return new ForgotPasswordResponse
        (
            Email: user.Email!,
            RequestedAt: DateTime.UtcNow
        );
    }
}