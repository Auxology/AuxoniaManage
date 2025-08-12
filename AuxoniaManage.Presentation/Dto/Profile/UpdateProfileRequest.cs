namespace AuxoniaManage.Presentation.Dto.Profile;

public sealed record UpdateProfileRequest
(
    string? FirstName,
    string? LastName,
    IFormFile? Avatar
);