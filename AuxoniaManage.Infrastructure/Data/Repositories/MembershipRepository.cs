using AuxoniaManage.Application.Features.Membership;
using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class MembershipRepository : IMembershipRepository
{
    private readonly ApplicationDbContext _context;


    public MembershipRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddAsync(Membership membership, CancellationToken cancellationToken)
    {
        await _context.Memberships.AddAsync(membership, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateAsync(Membership membership, CancellationToken cancellationToken)
    {
        _context.Memberships.Update(membership);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(Membership membership, CancellationToken cancellationToken)
    {
        _context.Memberships.Remove(membership);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteRangeAsync(IReadOnlyList<Membership> memberships, CancellationToken cancellationToken)
    {
        _context.Memberships.RemoveRange(memberships);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Membership?> GetSpecificAsync(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.WorkspaceId == workspaceId && m.UserId == userId, cancellationToken);
    }

    public async Task<IReadOnlyList<Membership>> GetSpecificsAsync(Guid workspaceId, IReadOnlyCollection<string> userIds, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .AsNoTracking()
            .Where(m => m.WorkspaceId == workspaceId && userIds.Contains(m.UserId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Membership>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .AsNoTracking()
            .Where(m => m.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Membership?>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .AsNoTracking()
            .Where(m => m.UserId == userId.ToString())
            .ToListAsync(cancellationToken);
    }
}