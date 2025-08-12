using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Workspace.Get;

public sealed class GetWorkspaceQueryHandler : IQueryHandler<GetWorkspaceQuery, GetWorkspaceResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IStorageService _storageService;

    public GetWorkspaceQueryHandler
    (
        IWorkspaceRepository workspaceRepository,
        IWorkspacePermissionService workspacePermissionService,
        IStorageService storageService
    )

    {
        _workspaceRepository = workspaceRepository;
        _workspacePermissionService = workspacePermissionService;
        _storageService = storageService;
    }

    public async Task<GetWorkspaceResponse> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            throw new WorkspaceNotFoundException(request.WorkspaceId);
        }
        
        var membership = await _workspacePermissionService.IsMemberAsync(
            request.WorkspaceId,
            request.UserId,
            cancellationToken
        );
        
        if (!membership)
        {
            throw new UserIsNotMemberException();
        }
        
        var logoUrl = string.IsNullOrEmpty(workspace.LogoKey)
            ? null
            : await _storageService.ConstructUrlAsync(workspace.LogoKey, cancellationToken);

        return new GetWorkspaceResponse
        (
            Id: workspace.Id,
            Name: workspace.Name,
            Description: workspace.Description,
            OwnerId: workspace.OwnerId,
            InvitationToken: workspace.InvitationToken,
            CreatedAt: workspace.CreatedAt,
            UpdatedAt: workspace.UpdatedAt,
            LogoUrl: logoUrl
        );
    }
}