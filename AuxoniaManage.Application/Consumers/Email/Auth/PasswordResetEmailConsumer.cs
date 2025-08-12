using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class PasswordResetEmailConsumer : IConsumer<PasswordResetEvent>
{
    private readonly ILogger<PasswordResetEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public PasswordResetEmailConsumer
    (
        ILogger<PasswordResetEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<PasswordResetEvent> context)
    {
        var message = context.Message;

        var request = new SendPasswordResetConfirmationRequest
        (
            Id: message.Id,
            Email: message.Email,
            ResetAt: message.ResetAt
        );
        
        _logger.LogInformation("Sending password reset confirmation email to {Email} for user {Id}", request.Email, request.Id);

        try
        {
            await _emailService.SendPasswordResetConfirmationAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Password reset confirmation email sent successfully to {Email} for user {Id}", request.Email, request.Id);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset confirmation email to {Email} for user {Id}", request.Email, request.Id);
            throw;
        }
    }
}