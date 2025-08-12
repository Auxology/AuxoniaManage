using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;

namespace AuxoniaManage.Application.Features.Workspace.DeleteReadModel;

public sealed class DeleteWorkspaceReadModelCommandHandler : ICommandHandler<DeleteWorkspaceReadModelCommand, DeleteWorkspaceReadModelResponse>
{
    private readonly ILogger<DeleteWorkspaceReadModelCommandHandler> _logger;
    private readonly IReadModelRepository _readModelRepository;

    public DeleteWorkspaceReadModelCommandHandler
    (
        ILogger<DeleteWorkspaceReadModelCommandHandler> logger,
        IReadModelRepository readModelRepository
    )
    {
        _logger = logger;
        _readModelRepository = readModelRepository;
    }

    public async Task<DeleteWorkspaceReadModelResponse> Handle(DeleteWorkspaceReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));

        _logger.LogInformation("Deleting workspace read model for WorkspaceId: {WorkspaceId}", request.WorkspaceId);

        var isDeleted = await _readModelRepository.DeleteWorkspaceAsync(request.WorkspaceId, cancellationToken);

        if (!isDeleted)
        {
            _logger.LogWarning("Workspace read model not found or failed to delete for WorkspaceId: {WorkspaceId}", request.WorkspaceId);
        }
        else
        {
            _logger.LogInformation("Successfully deleted workspace read model for WorkspaceId: {WorkspaceId}", request.WorkspaceId);
        }

        return new DeleteWorkspaceReadModelResponse(isDeleted, request.WorkspaceId);
    }
}