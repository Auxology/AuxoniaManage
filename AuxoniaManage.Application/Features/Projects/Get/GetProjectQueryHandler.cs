using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.Get;

public sealed class GetProjectQueryHandler : IQueryHandler<GetProjectQuery, GetProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IStorageService _storageService;
    
    public GetProjectQueryHandler
    (
        IProjectRepository projectRepository,
        IWorkspacePermissionService workspacePermissionService,
        IStorageService storageService
    )
    {
        _projectRepository = projectRepository;
        _workspacePermissionService = workspacePermissionService;
        _storageService = storageService;
    }
    
    public async Task<GetProjectResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Default(request.Id, nameof(request.Id));
        
        var hasPermission = await _workspacePermissionService.IsMemberAsync
        (
            request.WorkspaceId,
            request.UserId,
            cancellationToken
        );

        if (!hasPermission)
        {
            throw new UserDoesNotHavePermissionException();
        }
        
        var project = await _projectRepository.GetAsync(request.Id, cancellationToken);

        if (project == null)
        {
            throw new ProjectNotFoundException();
        }
        
        return new GetProjectResponse
        (
            project.Id,
            project.WorkspaceId,
            project.Name,
            project.CreatedAt,
            project.LogoKey != null
                ? await _storageService.ConstructUrlAsync(project.LogoKey, cancellationToken)
                : null
        );
    }
}