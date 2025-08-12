using AuxoniaManage.Domain.ReadModels;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class ReadModelRepository : IReadModelRepository
{
    private readonly ApplicationDbContext _context;
    
    public ReadModelRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddProfileAsync(ProfileReadModel profile, CancellationToken cancellationToken)
    {
        await _context.ProfileReadModels.AddAsync(profile, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateProfileAsync(ProfileReadModel profile, CancellationToken cancellationToken)
    {
        _context.ProfileReadModels.Update(profile);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<ProfileReadModel?> GetProfileAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.ProfileReadModels
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>p.UserId == userId, cancellationToken);
    }
    
    public async Task<bool> AddWorkspaceAsync(WorkspaceReadModel workspace, CancellationToken cancellationToken)
    {
        await _context.WorkspaceReadModels.AddAsync(workspace, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateWorkspaceAsync(WorkspaceReadModel workspace, CancellationToken cancellationToken)
    {
        _context.WorkspaceReadModels.Update(workspace);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<WorkspaceReadModel?> GetWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        return await _context.WorkspaceReadModels
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.WorkspaceId == workspaceId, cancellationToken);
    }

    public async Task<IReadOnlyList<WorkspaceReadModel>> GetWorkspacesAsync(IReadOnlyCollection<Guid> workspaceIds, CancellationToken cancellationToken)
    {
        return await _context.WorkspaceReadModels
            .AsNoTracking()
            .Where(w => workspaceIds.Contains(w.WorkspaceId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfileReadModel>> GetProfileByUserId(IReadOnlyCollection<string> userIds, CancellationToken cancellationToken)
    {
        return await _context.ProfileReadModels
            .AsNoTracking()
            .Where(p => userIds.Contains(p.UserId))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> AddProjectAsync(ProjectReadModel project, CancellationToken cancellationToken)
    {
        await _context.ProjectReadModels.AddAsync(project, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateProjectAsync(ProjectReadModel project, CancellationToken cancellationToken)
    {
        _context.ProjectReadModels.Update(project);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<ProjectReadModel?> GetProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.ProjectReadModels
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProjectId == projectId, cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectReadModel>> GetProjectsAsync(IReadOnlyCollection<Guid> projectIds, CancellationToken cancellationToken)
    {
        return await _context.ProjectReadModels
            .AsNoTracking()
            .Where(p => projectIds.Contains(p.ProjectId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectReadModel>> GetProjectsByWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        return await _context.ProjectReadModels
            .AsNoTracking()
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _context.ProjectReadModels
            .FirstOrDefaultAsync(p => p.ProjectId == projectId, cancellationToken);

        if (project is null)
            return false;

        _context.ProjectReadModels.Remove(project);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteProjectsAsync(IReadOnlyList<ProjectReadModel> projects, CancellationToken cancellationToken)
    {
        _context.ProjectReadModels.RemoveRange(projects);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }


    public async Task<bool> DeleteWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        var workspace = await _context.WorkspaceReadModels
            .FirstOrDefaultAsync(w => w.WorkspaceId == workspaceId, cancellationToken);

        if (workspace is null)
            return false;

        _context.WorkspaceReadModels.Remove(workspace);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}