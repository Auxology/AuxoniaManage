using AuxoniaManage.Application.Features.Workspace;
using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDbContext _context;
    
    public WorkspaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        await _context.Workspaces.AddAsync(workspace, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Workspace?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Workspaces
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        _context.Workspaces.Update(workspace);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        _context.Workspaces.Remove(workspace);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}