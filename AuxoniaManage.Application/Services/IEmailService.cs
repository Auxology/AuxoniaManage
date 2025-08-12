using AuxoniaManage.SharedKernel.Dto.Email.Auth;

namespace AuxoniaManage.Application.Services;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(SendVerificationEmailRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendSuccessfulLoginEmailAsync(SendSuccessfulLoginEmailRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendLockoutEmailAsync(SendLockoutEmailRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendSecurityWarningEmailAsync(SendSecurityWarningEmail request, CancellationToken cancellationToken);
    
    Task<bool> SendForgotPasswordEmailAsync(SendForgotPasswordEmailRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendEmailVerifiedConfirmationAsync(SendEmailVerifiedConfirmationRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendPasswordResetConfirmationAsync(SendPasswordResetConfirmationRequest request, CancellationToken cancellationToken);
    
    Task<bool> SendPasswordChangedEmailAsync(SendPasswordChangedEmailRequest request, CancellationToken cancellationToken);
}