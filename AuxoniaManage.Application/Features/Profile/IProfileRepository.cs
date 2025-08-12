using AuxoniaManage.Domain.Entities;

namespace AuxoniaManage.Application.Features.Profile;

public interface IProfileRepository
{
    Task<bool> AddAsync(UserProfile profile, CancellationToken cancellationToken);
    
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(UserProfile profile, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}