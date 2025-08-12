using AuxoniaManage.Domain.Entities;

namespace AuxoniaManage.Application.Features.Workspace;

public interface IWorkspaceRepository
{
    Task<bool> AddAsync(Domain.Entities.Workspace workspace, CancellationToken cancellationToken);
    
    Task<Domain.Entities.Workspace?> GetAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(Domain.Entities.Workspace workspace, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Domain.Entities.Workspace workspace, CancellationToken cancellationToken);
}