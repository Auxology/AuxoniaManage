namespace AuxoniaManage.Application.Features.ProjectTask;

public interface IProjectTaskRepository
{
    Task<bool> AddAsync(Domain.Entities.ProjectTask projectTask, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(Domain.Entities.ProjectTask projectTask, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Domain.Entities.ProjectTask projectTask, CancellationToken cancellationToken);
    
    Task<bool> DeleteRangeAsync(IReadOnlyList<Domain.Entities.ProjectTask> projectTasks, CancellationToken cancellationToken);
    
    Task<Domain.Entities.ProjectTask?> GetByIdAsync(Guid projectTaskId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.ProjectTask>> GetAllAsync(Guid projectId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.ProjectTask>> GetAssignedToUserAsync(Guid projectId, string userId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.ProjectTask>> GetByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.ProjectTask>> GetProjectTasksOfUserAsync(IReadOnlyCollection<Guid> projectIds, string userId, CancellationToken cancellationToken);
}