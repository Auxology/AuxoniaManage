using AuxoniaManage.Application.Features.Profile;
using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data.Repositories;

public sealed class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        await _context.UserProfiles.AddAsync(profile, cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        _context.UserProfiles.Update(profile);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _context.UserProfiles
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}