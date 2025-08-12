using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Features.Auth.Verify_Email;

public sealed class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, VerifyEmailResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public VerifyEmailCommandHandler
    (
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<VerifyEmailResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Token, nameof(request.Token));
        
        var user = await _userManager.FindByIdAsync(request.UserId);
        
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        
        if (!result.Succeeded)
        {            
            throw new EmailVerificationFailedException();
        }
        
        var emailVerifiedEvent = new EmailVerifiedEvent
        (
            Id: user.Id,
            Email: user.Email!,
            VerifiedAt: DateTime.UtcNow
        );
        
        await _publishEndpoint.Publish(emailVerifiedEvent, cancellationToken);
        
        return new VerifyEmailResponse
        (
            UserId: user.Id,
            VerifiedAt: DateTime.UtcNow
        );
    }
}