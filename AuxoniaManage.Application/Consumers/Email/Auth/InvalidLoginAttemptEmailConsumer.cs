using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class InvalidLoginAttemptEmailConsumer : IConsumer<InvalidLoginAttemptEvent>
{
    private readonly ILogger<InvalidLoginAttemptEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public InvalidLoginAttemptEmailConsumer
    (
        ILogger<InvalidLoginAttemptEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<InvalidLoginAttemptEvent> context)
    {
        var message = context.Message;

        var request = new SendSecurityWarningEmail
        (
            Id: message.Id,
            Email: message.Email,
            IpAddress: message.IpAddress,
            UserAgent: message.UserAgent,
            Timestamp: message.Timestamp,
            FailedAttempts: message.FailedAttempts
        );
        
        _logger.LogInformation("Sending security warning email to {Email} for user {Id} after {FailedAttempts} failed attempts", request.Email, request.Id, request.FailedAttempts);
        
        try
        {
            await _emailService.SendSecurityWarningEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Security warning email sent successfully to {Email} for user {Id} after {FailedAttempts} failed attempts", request.Email, request.Id, request.FailedAttempts);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send security warning email to {Email} for user {Id} after {FailedAttempts} failed attempts", request.Email, request.Id, request.FailedAttempts);
            throw;
        }
    }
}