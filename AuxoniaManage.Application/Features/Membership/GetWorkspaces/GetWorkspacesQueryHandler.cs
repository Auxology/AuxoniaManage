using Ardalis.GuardClauses;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Membership.GetWorkspaces;

public sealed class GetWorkspacesQueryHandler : IQueryHandler<GetWorkspacesQuery, GetWorkspacesResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IStorageService _storageService;

    public GetWorkspacesQueryHandler
    (
        IMembershipRepository membershipRepository,
        IReadModelRepository readModelRepository,
        IStorageService storageService
    )
    {
        _membershipRepository = membershipRepository;
        _readModelRepository = readModelRepository;
        _storageService = storageService;
    }

    public async Task<GetWorkspacesResponse> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        
        var workspaces = await _membershipRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (workspaces.Count == 0)
        {
            return new GetWorkspacesResponse([]);
        }
        
        var workspaceReadModels = await _readModelRepository.GetWorkspacesAsync
        (
            workspaces.Where(w => w != null).Select(w => w!.WorkspaceId).ToArray(),
            cancellationToken
        );

        var workspaceTasks = workspaceReadModels.Select(async w => new WorkspaceDto
        (
            w.Id,
            w.Name,
            w.LogoKey != null
                ? await _storageService.ConstructUrlAsync(w.LogoKey, cancellationToken)
                : null
        ));
        
        var workspaceDtos = await Task.WhenAll(workspaceTasks);
        
        return new GetWorkspacesResponse(workspaceDtos.ToList());
    }
}