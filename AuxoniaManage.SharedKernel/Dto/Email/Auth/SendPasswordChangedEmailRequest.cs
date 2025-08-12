namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendPasswordChangedEmailRequest
(
    string UserId,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime ChangedAt
);