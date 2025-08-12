using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Workspace.UpdateOwner;

public sealed class UpdateWorkspaceOwnerCommandHandler : ICommandHandler<UpdateWorkspaceOwnerCommand, UpdateWorkspaceOwnerResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    
    public UpdateWorkspaceOwnerCommandHandler(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }


    public async Task<UpdateWorkspaceOwnerResponse> Handle(UpdateWorkspaceOwnerCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.NewOwnerId, nameof(request.NewOwnerId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));

        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            throw new WorkspaceNotFoundException();
        }
        
        workspace.UpdateOwner(request.NewOwnerId, DateTime.UtcNow);
        
        var isSuccess = await _workspaceRepository.UpdateAsync(workspace, cancellationToken);
        
        if (!isSuccess)
        {
            throw new WorkspaceOwnershipTransferFailedException();
        }
        
        return new UpdateWorkspaceOwnerResponse
        (
            request.NewOwnerId,
            request.WorkspaceId,
            workspace.UpdatedAt
        );
    }
}