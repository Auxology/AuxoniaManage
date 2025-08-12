using AuxoniaManage.Application.Features.ProjectTask;
using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProjectTaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<bool> AddAsync(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        await _context.ProjectTasks.AddAsync(projectTask, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateAsync(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        _context.ProjectTasks.Update(projectTask);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        _context.ProjectTasks.Remove(projectTask);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteRangeAsync(IReadOnlyList<ProjectTask> projectTasks, CancellationToken cancellationToken)
    {
        _context.ProjectTasks.RemoveRange(projectTasks);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<ProjectTask?> GetByIdAsync(Guid projectTaskId, CancellationToken cancellationToken)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == projectTaskId, cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectTask>> GetAllAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .Where(pt => pt.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<ProjectTask>> GetAssignedToUserAsync(Guid projectId, string userId, CancellationToken cancellationToken)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.AssigneeIds.Contains(userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectTask>> GetByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds, CancellationToken cancellationToken)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .Where(pt => projectIds.Contains(pt.ProjectId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectTask>> GetProjectTasksOfUserAsync(IReadOnlyCollection<Guid> projectIds, string userId, CancellationToken cancellationToken)
    {
        return await _context.ProjectTasks
            .AsNoTracking()
            .Where(x => 
                    projectIds.Contains(x.ProjectId) 
                    && (x.AssigneeIds.Contains(userId) 
                    && x.AssigneeIds.Count == 1 
                    || x.AssignedById == userId))
            .ToListAsync(cancellationToken);
    }
}