using AuxoniaManage.Domain.Entities;

namespace AuxoniaManage.Application.Features.Projects;

public interface IProjectRepository
{
    Task<bool> AddAsync(Project project, CancellationToken cancellationToken);
    
    Task<Project?> GetAsync(Guid projectId, CancellationToken cancellationToken);
    
    Task<Project?> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Project>> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(Project project, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Project project, CancellationToken cancellationToken);
    
    Task<bool> DeleteRangeAsync(IReadOnlyList<Project> projects, CancellationToken cancellationToken);
}