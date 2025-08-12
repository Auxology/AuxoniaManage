using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class EmailVerifiedEmailConsumer : IConsumer<EmailVerifiedEvent>
{
    private readonly ILogger<EmailVerifiedEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public EmailVerifiedEmailConsumer
    (
        ILogger<EmailVerifiedEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<EmailVerifiedEvent> context)
    {
        var message = context.Message;

        var request = new SendEmailVerifiedConfirmationRequest
        (
            Id: message.Id,
            Email: message.Email,
            VerifiedAt: message.VerifiedAt
        );
        
        _logger.LogInformation("Sending email verification confirmation to {Email} for user {Id}", request.Email, request.Id);

        try
        {
            await _emailService.SendEmailVerifiedConfirmationAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Email verification confirmation sent successfully to {Email} for user {Id}", request.Email, request.Id);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email verification confirmation to {Email} for user {Id}", request.Email, request.Id);
            throw;
        }
    }
}