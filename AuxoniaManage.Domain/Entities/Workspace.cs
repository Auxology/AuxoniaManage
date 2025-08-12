namespace AuxoniaManage.Domain.Entities;

public sealed class Workspace
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public string OwnerId { get; private set; }
    
    public string InvitationToken { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public string? LogoKey { get; private set; }
    
    private Workspace()
    {
        // Required for EF Core
    }
    
    public Workspace(string name, string description, string ownerId, DateTime timeStamp, string invitationToken, string? logoKey = null)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
        InvitationToken = invitationToken;
        CreatedAt = timeStamp;
        UpdatedAt = timeStamp;
        LogoKey = logoKey;
    }
    
    public void UpdateWorkspace(string name, string description, DateTime timeStamp, string? logoKey = null)
    {
        Name = name;
        Description = description;
        UpdatedAt = timeStamp;
        LogoKey = logoKey;
    }
    
    public void UpdateInvitationToken(string newToken, DateTime timeStamp)
    {
        InvitationToken = newToken;
        UpdatedAt = timeStamp;
    }
    
    public void UpdateOwner(string newOwnerId, DateTime timeStamp)
    {
        OwnerId = newOwnerId;
        UpdatedAt = timeStamp;
    }
}