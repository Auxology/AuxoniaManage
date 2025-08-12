namespace AuxoniaManage.Presentation.Dto.Project;

public record UpdateProjectRequest
(
    string? Name,
    IFormFile? Logo
);