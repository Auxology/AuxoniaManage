using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class PasswordChangedEmailConsumer : IConsumer<PasswordChangedEvent>
{
    private readonly ILogger<PasswordChangedEmailConsumer> _logger;
    private readonly IEmailService _emailService;
    
    public PasswordChangedEmailConsumer
    (
        ILogger<PasswordChangedEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<PasswordChangedEvent> context)
    {
        var message = context.Message;
        
        var request = new SendPasswordChangedEmailRequest
        (
            UserId: message.UserId,
            Email: message.Email,
            IpAddress: message.IpAddress,
            UserAgent: message.UserAgent,
            ChangedAt: message.ChangedAt
        );
        
        _logger.LogInformation("Sending password changed notification email to {Email} for user {UserId} changed at {ChangedAt}", request.Email, request.UserId, request.ChangedAt);

        try
        {
            await _emailService.SendPasswordChangedEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Password changed notification email sent successfully to {Email} for user {UserId}", request.Email, request.UserId);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password changed notification email to {Email} for user {UserId}", request.Email, request.UserId);
            throw;
        }
    }
}