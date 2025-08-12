using AuxoniaManage.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace AuxoniaManage.Application.Services;

public interface IWorkspacePermissionService
{
    Task<bool> IsOwnerAsync(Guid workspaceId, string userId, CancellationToken cancellationToken);
    
    Task<bool> IsAdminAsync(Guid workspaceId, string userId, CancellationToken cancellationToken);
    
    Task<bool> IsMemberAsync(Guid workspaceId, string userId, CancellationToken cancellationToken);
    
    Task<WorkspaceRoles> GetRoleAsync(Guid workspaceId, string userId, CancellationToken cancellationToken);
    
    Task EnsureHierarchyAsync(Guid workspaceId, string userId, IReadOnlyList<string> userIds, CancellationToken cancellationToken);
}