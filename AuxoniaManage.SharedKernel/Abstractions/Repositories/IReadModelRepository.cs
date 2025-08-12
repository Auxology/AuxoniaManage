using AuxoniaManage.Domain.Entities;
using AuxoniaManage.Domain.ReadModels;

namespace AuxoniaManage.SharedKernel.Abstractions.Repositories;

public interface IReadModelRepository
{
    Task<bool> AddProfileAsync(ProfileReadModel profile, CancellationToken cancellationToken);
    
    Task<bool> UpdateProfileAsync(ProfileReadModel profile, CancellationToken cancellationToken);
    
    Task<ProfileReadModel?> GetProfileAsync(string userId, CancellationToken cancellationToken);
    
    Task<bool> AddWorkspaceAsync(WorkspaceReadModel workspace, CancellationToken cancellationToken);
    
    Task<bool> UpdateWorkspaceAsync(WorkspaceReadModel workspace, CancellationToken cancellationToken);
    
    Task<WorkspaceReadModel?> GetWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<WorkspaceReadModel>> GetWorkspacesAsync(IReadOnlyCollection<Guid> workspaceIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<ProfileReadModel>> GetProfileByUserId(IReadOnlyCollection<string> userIds, CancellationToken cancellationToken);
    
    Task<bool> AddProjectAsync(ProjectReadModel project, CancellationToken cancellationToken);
    
    Task<bool> UpdateProjectAsync(ProjectReadModel project, CancellationToken cancellationToken);
    
    Task<ProjectReadModel?> GetProjectAsync(Guid projectId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<ProjectReadModel>> GetProjectsAsync(IReadOnlyCollection<Guid> projectIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<ProjectReadModel>> GetProjectsByWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectAsync(Guid projectId, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectsAsync(IReadOnlyList<ProjectReadModel> projects, CancellationToken cancellationToken);
    
    Task<bool> DeleteWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken);
}