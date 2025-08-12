namespace AuxoniaManage.Presentation.Dto.Auth;

public sealed record ChangePasswordRequest
(
    string OldPassword,
    string NewPassword
);