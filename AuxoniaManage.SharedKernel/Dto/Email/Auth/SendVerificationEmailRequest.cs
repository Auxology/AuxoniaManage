namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendVerificationEmailRequest
(
    string Id,
    string FullName,
    string Email,
    string VerificationToken
);