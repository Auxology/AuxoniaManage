using System.Text.Encodings.Web;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Infrastructure.Email.Configs;
using AuxoniaManage.SharedKernel.Dto.Email.Auth;
using Microsoft.Extensions.Options;

namespace AuxoniaManage.Infrastructure.Email.Services;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IAmazonSimpleEmailService _client;

    public EmailService
    (
        IOptions<EmailSettings> emailSettings,
        IAmazonSimpleEmailService client
    )
    {
        _emailSettings = emailSettings.Value;
        _client = client;
    }
    
    public async Task<bool> SendVerificationEmailAsync(SendVerificationEmailRequest request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Email Verification"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Email Verification</h1>
                                    <p>Account with Id:{request.Id} was create with this email address.</p>
                                    <p>To verify your email, please click the link below:</p>
                                    <p><a href='{_emailSettings.BaseUrl}?userId={request.Id}&token={Uri.EscapeDataString(request.VerificationToken)}'>Verify Email</a></p>
                                    <p>If you did not create this account, please contact support.</p>                                 
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendSuccessfulLoginEmailAsync(SendSuccessfulLoginEmailRequest request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Successful Login Notification"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Successful Login</h1>
                                    <p>Your account with Id: {request.Id} has been successfully logged in.</p>
                                    <p>Details:</p>
                                    <ul>
                                        <li>IP Address: {request.IpAddress}</li>
                                        <li>User Agent: {request.UserAgent}</li>
                                        <li>Login Time: {request.LoginTime}</li>
                                    </ul>
                                    <p>If this was not you, please change password immediately.</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendLockoutEmailAsync(SendLockoutEmailRequest request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Account Locked"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Account Locked</h1>
                                    <p>Your account with Id: {request.Id} has been locked due to suspicious activity.</p>
                                    <p>Details:</p>
                                    <ul>
                                        <li>IP Address: {request.IpAddress}</li>
                                        <li>User Agent: {request.UserAgent}</li>
                                        <li>Locked At: {request.LockedAt}</li>
                                    </ul>
                                    <p>If you believe this is a mistake, please contact support.</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }
    
    public async Task<bool> SendSecurityWarningEmailAsync(SendSecurityWarningEmail request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Security Warning"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Security Warning</h1>
                                    <p>We detected suspicious activity on your account with Id: {request.Id}.</p>
                                    <p>Details:</p>
                                    <ul>
                                        <li>IP Address: {request.IpAddress}</li>
                                        <li>User Agent: {request.UserAgent}</li>
                                        <li>Date and Time: {request.Timestamp}</li>
                                        <li>Attempt Count: {request.FailedAttempts}</li>
                                    </ul>
                                    <p>If this was not you, please change your password immediately.</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendForgotPasswordEmailAsync(SendForgotPasswordEmailRequest request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Forgot Password Request"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Forgot Password</h1>
                                    <p>We received a request to reset the password for your account with Id: {request.Id}.</p>
                                    <p>To reset your password, please click the link below:</p>
                                    <p><a href='{_emailSettings.BaseUrl}?userId={request.Id}&token={Uri.EscapeDataString(request.ResetToken)}'>Reset Password</a></p>
                                    <p>If you did not request this, please ignore this email.</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendEmailVerifiedConfirmationAsync(SendEmailVerifiedConfirmationRequest request,
        CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Email Verified"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Email Verified</h1>
                                    <p>Your email with Id: {request.Id} has been successfully verified.</p>
                                    <p>Verified At: {request.VerifiedAt}</p>
                                    <p>Thank you for verifying your email!</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendPasswordResetConfirmationAsync(SendPasswordResetConfirmationRequest request,
        CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Password Reset Confirmation"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Password Reset Successful</h1>
                                    <p>Your password for account with Id: {request.Id} has been successfully reset.</p>
                                    <p>Reset At: {request.ResetAt}</p>
                                    <p>If you did not request this, please contact support immediately.</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);

        return true;
    }

    public async Task<bool> SendPasswordChangedEmailAsync(SendPasswordChangedEmailRequest request, CancellationToken cancellationToken)
    {
        var emailRequest = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            },
            Message = new Message
            {
                Subject = new Content("Password Changed"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Data = $@"
                            <html>
                                <body>
                                    <h1>Password Changed</h1>
                                    <p>Your password for account with Id: {request.UserId} has been successfully changed.</p>
                                    <p>Changed At: {request.ChangedAt}</p>
                                    <p>If you did not initiate this change, please contact support immediately.</p>
                                    <p>Account Email: {request.Email}</p>
                                    <p>IP Address: {request.IpAddress}</p>
                                    <p>User Agent: {request.UserAgent}</p>
                                </body>
                            </html>"
                    }
                }
            }
        };
        
        await _client.SendEmailAsync(emailRequest, cancellationToken);
        
        return true;
    }
}