using AuxoniaManage.Application.Features.Projects;
using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddAsync(Project project, CancellationToken cancellationToken)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Project?> GetAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
    }

    public async Task<Project?> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.WorkspaceId == workspaceId, cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Project project, CancellationToken cancellationToken)
    {
        _context.Projects.Update(project);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(Project project, CancellationToken cancellationToken)
    {
        _context.Projects.Remove(project);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteRangeAsync(IReadOnlyList<Project> projects, CancellationToken cancellationToken)
    {
        _context.Projects.RemoveRange(projects);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}