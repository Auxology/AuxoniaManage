namespace AuxoniaManage.Presentation.Dto.Workspace;

public sealed record CreateWorkspaceRequest
(
    string Name,
    string Description,
    IFormFile? Logo
);