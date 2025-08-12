namespace AuxoniaManage.Presentation.Dto.Workspace;

public sealed record UpdateWorkspaceRequest
(
    string? Name,
    string? Description,
    IFormFile? Logo
);