using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.GetMany;

public sealed class GetManyProjectsQueryHandler : IQueryHandler<GetManyProjectsQuery, GetManyQueryResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStorageService _storageService;
    private readonly IWorkspacePermissionService _workspacePermissionService;

    public GetManyProjectsQueryHandler
    (
        IProjectRepository projectRepository,
        IStorageService storageService,
        IWorkspacePermissionService workspacePermissionService
    )

    {
        _projectRepository = projectRepository;
        _storageService = storageService;
        _workspacePermissionService = workspacePermissionService;
    }

    public async Task<GetManyQueryResponse> Handle(GetManyProjectsQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var hasPermission = await _workspacePermissionService.IsMemberAsync(
            request.WorkspaceId,
            request.UserId,
            cancellationToken
        );

        if (!hasPermission)
        {
            throw new UserDoesNotHavePermissionException();
        }
        
        var projects = await _projectRepository.GetAllAsync(
            request.WorkspaceId,
            cancellationToken
        );

        if (projects.Count == 0)
        {
            return new GetManyQueryResponse([]);
        }
        
        var projectTasks = projects.Select(async project => new ProjectDto(
            project.Id,
            project.Name,
            project.LogoKey != null 
                ? await _storageService.ConstructUrlAsync(project.LogoKey, cancellationToken) 
                : null,
            project.WorkspaceId
        )).ToArray();
        
        var projectDtos = await Task.WhenAll(projectTasks);
        
        return new GetManyQueryResponse(projectDtos.ToList());
    }
}