using System.Security.Claims;
using AuxoniaManage.Application.Features.Auth.Change_Password;
using AuxoniaManage.Application.Features.Auth.Forgot_Password;
using AuxoniaManage.Application.Features.Auth.Login;
using AuxoniaManage.Application.Features.Auth.Logout;
using AuxoniaManage.Application.Features.Auth.Reset_Password;
using AuxoniaManage.Application.Features.Auth.Verify_Email;
using AuxoniaManage.Application.Features.Onboarding.CreateUser;
using AuxoniaManage.Presentation.Dto.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        
        var loginCommand = new LoginCommand
        (
            Email: request.Email,
            Password: request.Password,
            IpAddress: ipAddress!,
            UserAgent: userAgent,
            RememberMe: request.RememberMe
        );
        
        var response = await _mediator.Send(loginCommand);
        
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var logoutCommand = new LogoutCommand(userId);
        
        var response = await _mediator.Send(logoutCommand);
        
        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var createUserCommand = new CreateUserCommand
        (
            Email: request.Email,
            Password: request.Password,
            FirstName: request.FirstName,
            LastName: request.LastName
        );
        
        var response = await _mediator.Send(createUserCommand);

        return Ok(response);
    }

    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var decodeToken = Uri.UnescapeDataString(token);
        
        var verifyEmailCommand = new VerifyEmailCommand
        (
            UserId: userId,
            Token: decodeToken
        );
        
        var response = await _mediator.Send(verifyEmailCommand);
        
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var forgotPasswordCommand = new ForgotPasswordCommand(request.Email);
        
        var response = await _mediator.Send(forgotPasswordCommand);
        
        return Ok(response);
    }

    [HttpPatch("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, [FromQuery] string userId, [FromQuery] string token)
    {
        var decodedToken = Uri.UnescapeDataString(token);
        
        var resetPasswordCommand = new ResetPasswordCommand
        (
            UserId: userId,
            Token: decodedToken,
            NewPassword: request.NewPassword
        );
        
        var response = await _mediator.Send(resetPasswordCommand);
        
        return Ok(response);
    }
    
    [HttpPatch("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var changePasswordCommand = new ChangePasswordCommand
        (
            IpAddress: ipAddress!,
            UserAgent: userAgent,
            UserId: userId,
            OldPassword: request.OldPassword,
            NewPassword: request.NewPassword
        );
        
        var response = await _mediator.Send(changePasswordCommand);
        
        return Ok(response);
    }
}