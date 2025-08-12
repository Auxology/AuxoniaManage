namespace AuxoniaManage.Application.Features.Membership;

public interface IMembershipRepository
{
    Task<bool> AddAsync(Domain.Entities.Membership membership, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(Domain.Entities.Membership membership, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Domain.Entities.Membership membership, CancellationToken cancellationToken);
    
    Task<bool> DeleteRangeAsync(IReadOnlyList<Domain.Entities.Membership> memberships, CancellationToken cancellationToken);
    
    Task<Domain.Entities.Membership?> GetSpecificAsync(Guid workspaceId, string userId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.Membership>> GetSpecificsAsync(Guid workspaceId, IReadOnlyCollection<string> userIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.Membership>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Domain.Entities.Membership?>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
}