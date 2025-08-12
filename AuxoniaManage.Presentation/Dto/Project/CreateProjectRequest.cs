namespace AuxoniaManage.Presentation.Dto.Project;

public record CreateProjectRequest
(
    string Name,
    IFormFile? Logo
);