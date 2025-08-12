using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.ReadModels;

namespace AuxoniaManage.Domain.Entities;

public sealed class Membership
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public Guid WorkspaceId { get; private set; }
    
    public WorkspaceRoles Role { get; private set; }
    
    public DateTime JoinedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    
    private Membership()
    {
        // Required for EF Core
    }
    
    public Membership(string userId, Guid workspaceId, WorkspaceRoles role, DateTime timeStamp)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        Role = role;
        JoinedAt = timeStamp;
        UpdatedAt = timeStamp;
    }

    public void UpdateMembership(WorkspaceRoles role, DateTime timeStamp)
    {
        Role = role;
        UpdatedAt = timeStamp;
    }
}