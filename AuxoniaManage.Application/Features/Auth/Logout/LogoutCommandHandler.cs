using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuxoniaManage.Application.Features.Auth.Logout;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, LogoutResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutCommandHandler
    (
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
    )
    
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        
        var user = await _userManager.FindByIdAsync(request.UserId);
        
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        await _signInManager.SignOutAsync();
        
        return new LogoutResponse
        (
            Id: user.Id,
            LoggedOutAt: DateTime.UtcNow
        );
    }
}