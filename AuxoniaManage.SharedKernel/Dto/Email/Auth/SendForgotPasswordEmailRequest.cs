namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendForgotPasswordEmailRequest
(
    string Id,
    string Email,
    string ResetToken,
    DateTime RequestedAt
);