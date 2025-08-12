namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendSecurityWarningEmail
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime Timestamp,
    int FailedAttempts
);