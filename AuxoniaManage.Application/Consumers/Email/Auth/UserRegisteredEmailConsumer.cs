using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Auth;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Email.Auth;

public sealed class UserRegisteredEmailConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public UserRegisteredEmailConsumer
    (
        ILogger<UserRegisteredEmailConsumer> logger,
        IEmailService emailService
    )
    
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var message = context.Message;

        var request = new SendVerificationEmailRequest
        (
            Id: message.Id,
            Email: message.Email,
            FullName: message.FullName,
            VerificationToken: message.VerificationToken
        );
        
        _logger.LogInformation("Sending verification email to {Email} for user {FullName}", request.Email, request.FullName);

        try
        {
            await _emailService.SendVerificationEmailAsync(request, context.CancellationToken);
            
            _logger.LogInformation("Successfully sent verification email to {Email} for user {FullName}", request.Email, request.FullName);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email} for user {FullName}", request.Email, request.FullName);
            throw;
        }
    }
}