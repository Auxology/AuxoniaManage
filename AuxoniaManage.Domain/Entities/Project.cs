namespace AuxoniaManage.Domain.Entities;

public sealed class Project
{
    public Guid Id { get; private set; }
    
    public Guid WorkspaceId { get; private set; }
    
    public string Name { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public string? LogoKey { get; private set; }
    
    private Project()
    {
        // Required for EF Core
    }
    
    public Project(Guid workspaceId, string name, DateTime timeStamp, string? logoKey = null)
    {
        WorkspaceId = workspaceId;
        Name = name;
        CreatedAt = timeStamp;
        UpdatedAt = timeStamp;
        LogoKey = logoKey;
    }
    
    public void UpdateProject(string name, string? logoKey, DateTime timeStamp)
    {
        Name = name;
        LogoKey = logoKey;
        UpdatedAt = timeStamp;
    }
}