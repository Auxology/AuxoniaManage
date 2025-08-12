namespace AuxoniaManage.Domain.ReadModels;

public sealed class ProjectReadModel
{
    public Guid Id { get; private set; }
    
    public Guid ProjectId { get; private set; }
    
    public string Name { get; private set; }
    
    public Guid WorkspaceId { get; private set; }
    
    public string? LogoKey { get; private set; }
    
    private ProjectReadModel() { }

    public ProjectReadModel
    (
        Guid projectId,
        string name,
        Guid workspaceId,
        string? logoKey
    )
    
    {
        ProjectId = projectId;
        Name = name;
        WorkspaceId = workspaceId;
        LogoKey = logoKey;
    }
    
    public void UpdateProject
    (
        string name,
        string? logoKey
    )
    {
        Name = name;
        LogoKey = logoKey;
    }
}