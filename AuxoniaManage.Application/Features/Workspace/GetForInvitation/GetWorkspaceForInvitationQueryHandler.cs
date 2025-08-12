using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.GetForInvitation;

public sealed class GetWorkspaceForInvitationQueryHandler : IQueryHandler<GetWorkspaceForInvitationQuery, GetWorkspaceForInvitationResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IStorageService _storageService;

    public GetWorkspaceForInvitationQueryHandler
    (
        IWorkspaceRepository workspaceRepository,
        IStorageService storageService
    )

    {
        _workspaceRepository = workspaceRepository;
        _storageService = storageService;
    }

    public async Task<GetWorkspaceForInvitationResponse> Handle(GetWorkspaceForInvitationQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.InvitationToken, nameof(request.InvitationToken));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            throw new WorkspaceNotFoundException(request.WorkspaceId);
        }

        if (workspace.InvitationToken != request.InvitationToken)
        {
            throw new InvalidInvitationTokenException(request.InvitationToken);
        }
        
        var logoUrl = string.IsNullOrEmpty(workspace.LogoKey)
            ? null
            : await _storageService.ConstructUrlAsync(workspace.LogoKey, cancellationToken);

        return new GetWorkspaceForInvitationResponse
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