namespace AuxoniaManage.Domain.ReadModels;

public sealed class WorkspaceReadModel
{
    public Guid Id { get; private set; }
    
    public Guid WorkspaceId { get; private set; }
    
    public string Name { get; private set; }
    
    public string? LogoKey { get; private set; }
    
    private WorkspaceReadModel() { }
    
    public WorkspaceReadModel(Guid workspaceId, string name, string? logoKey)
    {
        WorkspaceId = workspaceId;
        Name = name;
        LogoKey = logoKey;
    }
    
    public void UpdateReadModel(string name, string? logoKey)
    {
        Name = name;
        LogoKey = logoKey;
    }
}