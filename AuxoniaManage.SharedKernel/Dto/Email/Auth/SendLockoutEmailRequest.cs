namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendLockoutEmailRequest
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime LockedAt
);