namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendSuccessfulLoginEmailRequest(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime LoginTime
);