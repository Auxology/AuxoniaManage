using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class ForgotPasswordEmailConsumer : IConsumer<ForgotPasswordEvent>
{
    private readonly ILogger<ForgotPasswordEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public ForgotPasswordEmailConsumer
    (
        ILogger<ForgotPasswordEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
    {
        var message = context.Message;

        var request = new SendForgotPasswordEmailRequest
        (
            Id: message.Id,
            Email: message.Email,
            ResetToken: message.ResetToken,
            RequestedAt: message.RequestedAt
        );
        
        _logger.LogInformation("Sending forgot password email to {Email} for user {Id}", request.Email, request.Id);

        try
        {
            await _emailService.SendForgotPasswordEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Forgot password email sent successfully to {Email} for user {Id}", request.Email, request.Id);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send forgot password email to {Email} for user {Id}", request.Email, request.Id);
            throw;
        }
    }
}