using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class SuccessfulLoginEmailConsumer : IConsumer<SuccessfulLoginEvent>
{
    private readonly ILogger<SuccessfulLoginEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public SuccessfulLoginEmailConsumer
    (
        ILogger<SuccessfulLoginEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<SuccessfulLoginEvent> context)
    {
        var message = context.Message;

        var request = new SendSuccessfulLoginEmailRequest
        (
            Id: message.Id,
            Email: message.Email,
            IpAddress: message.IpAddress,
            UserAgent: message.UserAgent,
            LoginTime: message.LoginTime
        );
        
        _logger.LogInformation("Sending successful login notification email to {Email} for user {Id} at {LoginTime}", request.Email, request.Id, request.LoginTime);

        try
        {
            await _emailService.SendSuccessfulLoginEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Successfully sent successful login notification email to {Email} for user {Id} at {LoginTime}", request.Email, request.Id, request.LoginTime);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send successful login notification email to {Email} for user {Id} at {LoginTime}", request.Email, request.Id, request.LoginTime);
            throw;
        }
    }
}