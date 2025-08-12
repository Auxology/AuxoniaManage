namespace AuxoniaManage.SharedKernel.Dto.Email.Auth;

public sealed record SendPasswordResetConfirmationRequest
(
    string Id,
    string Email,
    DateTime ResetAt
);