namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendEmailVerifiedConfirmationRequest
(
    string Id,
    string Email,
    DateTime VerifiedAt
);