using Ardalis.GuardClauses;
using AuxoniaManage.Application.Features.Projects;
using AuxoniaManage.Application.Features.ProjectTask;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Services;

public sealed class CleanUpService : ICleanUpService
{
    private readonly ILogger<CleanUpService> _logger;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    
    public CleanUpService
    (
        ILogger<CleanUpService> logger,
        IProjectRepository projectRepository,
        IProjectTaskRepository projectTaskRepository
    )
    {
        _logger = logger;
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
    }
    
    public async Task<bool> CleanUpProjectTasks(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        Guard.Against.Default(workspaceId, nameof(workspaceId));
        Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
        
        var projects = await _projectRepository.GetAllAsync(workspaceId, cancellationToken);
        
        if (projects.Count == 0)
        {
            _logger.LogInformation("No projects found for workspace {WorkspaceId}", workspaceId);
            return true;
        }
        
        var projectIds = projects.Select(p => p.Id).ToList();
        
        _logger.LogInformation("Found {ProjectCount} projects for workspace {WorkspaceId}", projects.Count, workspaceId);
        
        var allTasksToDelete = await _projectTaskRepository.GetProjectTasksOfUserAsync(projectIds, userId, cancellationToken);
        
        if (allTasksToDelete.Count == 0)
        {
            _logger.LogInformation("No project tasks found for user {UserId} in workspace {WorkspaceId}", userId, workspaceId);
            return true;
        }
        
        var isSuccess = await _projectTaskRepository.DeleteRangeAsync(allTasksToDelete, cancellationToken);

        if (!isSuccess)
        {
            _logger.LogError("Failed to delete project tasks for user {UserId} in workspace {WorkspaceId}", userId, workspaceId);
            return false;
        }
        
        _logger.LogInformation("Successfully deleted {TaskCount} project tasks for user {UserId} in workspace {WorkspaceId}", 
            allTasksToDelete.Count, userId, workspaceId);
        return true;
    }
}