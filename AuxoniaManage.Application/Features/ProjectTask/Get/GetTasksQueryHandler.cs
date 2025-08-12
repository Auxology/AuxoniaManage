using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.ProjectTask.Get;

public sealed class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, GetTasksResponse>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IStorageService _storageService;
    
    public GetTasksQueryHandler
    (
        IProjectTaskRepository projectTaskRepository,
        IReadModelRepository readModelRepository,
        IWorkspacePermissionService workspacePermissionService,
        IStorageService storageService
    )
    {
        _projectTaskRepository = projectTaskRepository;
        _readModelRepository = readModelRepository;
        _workspacePermissionService = workspacePermissionService;
        _storageService = storageService;
    }

    public async Task<GetTasksResponse> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        
        var workspaceReadModel = await _readModelRepository.GetWorkspaceAsync(request.WorkspaceId, cancellationToken);
        
        if (workspaceReadModel == null)
        {
            throw new CouldNotFindWorkspaceException();
        }
        
        var projectReadModel = await _readModelRepository.GetProjectAsync(request.ProjectId, cancellationToken);
        
        if (projectReadModel == null || projectReadModel.WorkspaceId != request.WorkspaceId)
        {
            throw new CouldNotFindProjectException();
        }

        var userRole =
            await _workspacePermissionService.GetRoleAsync(request.WorkspaceId, request.UserId, cancellationToken);
        
        var projectTasks = await GetTasksBasedOnRole(request.UserId, request.ProjectId, userRole, cancellationToken);
        
        var allUserIds = projectTasks.SelectMany(t => t.AssigneeIds.Append(t.AssignedById))
            .Append(request.UserId)
            .Distinct()
            .ToList();
        
        var profileReadModels = await _readModelRepository.GetProfileByUserId(allUserIds, cancellationToken);
        
        var profileReadModelDict = new Dictionary<string, ProfileDto>();

        foreach (var prm in profileReadModels)
        {
            var avatarUrl = prm.AvatarKey != null 
                ? await _storageService.ConstructUrlAsync(prm.AvatarKey, cancellationToken)
                : null;
            
            var fullName = prm.UserId == request.UserId ? "You" : prm.FullName;
            
            var profileDto = new ProfileDto
            (
                UserId: prm.UserId,
                FullName: fullName,
                AvatarUrl: avatarUrl
            );
           
            
            profileReadModelDict[prm.UserId] = profileDto;
        }
        
        var projectLogoUrl = projectReadModel.LogoKey != null
            ? await _storageService.ConstructUrlAsync(projectReadModel.LogoKey, cancellationToken)
            : null;

        var projectTasksDto = projectTasks.Select(t =>
        {
            return new ProjectTaskDto
            (
                Id: t.Id,
                ProjectId: projectReadModel.ProjectId,
                ProjectName: projectReadModel.Name,
                ProjectLogoUrl: projectLogoUrl,
                AssignedBy: profileReadModelDict[t.AssignedById],
                Assignees: t.AssigneeIds
                    .Where(id => profileReadModelDict.ContainsKey(id))
                    .Select(id => profileReadModelDict[id])
                    .ToList(),
                Title: t.Title,
                Description: t.Description,
                DueDate: t.DeadlineAt,
                Priority: t.Priority,
                Status: t.Status,
                CreatedAt: t.CreatedAt
            );
        }).ToList();
        
        return new GetTasksResponse(projectTasksDto);
    }

    private async Task<IReadOnlyList<Domain.Entities.ProjectTask>> GetTasksBasedOnRole(string userId, Guid projectId,
         WorkspaceRoles role, CancellationToken cancellationToken)
    {
        return role switch
        {
            WorkspaceRoles.Admin or WorkspaceRoles.Owner =>
                await _projectTaskRepository.GetAllAsync(projectId, cancellationToken),
            WorkspaceRoles.Member => await _projectTaskRepository.GetAssignedToUserAsync(projectId, userId,
                cancellationToken),
            _ => throw new InvalidOperationException("Invalid workspace role.")
        };
    }
}