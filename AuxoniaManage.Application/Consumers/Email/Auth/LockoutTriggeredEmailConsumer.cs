using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class LockoutTriggeredEmailConsumer : IConsumer<LockoutTriggeredEvent>
{
    private readonly ILogger<LockoutTriggeredEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public LockoutTriggeredEmailConsumer
    (
        ILogger<LockoutTriggeredEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<LockoutTriggeredEvent> context)
    {
        var message = context.Message;

        var request = new SendLockoutEmailRequest
        (
            Id: message.Id,
            Email: message.Email,
            IpAddress: message.IpAddress,
            UserAgent: message.UserAgent,
            LockedAt: message.LockedAt
        );
        
        _logger.LogInformation("Sending lockout notification email to {Email} for user {Id} locked at {LockedAt}", request.Email, request.Id, request.LockedAt);

        try
        {
            await _emailService.SendLockoutEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Lockout notification email sent successfully to {Email} for user {Id}", request.Email, request.Id);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send lockout notification email to {Email} for user {Id}", request.Email, request.Id);
            throw;
        }
    }
}