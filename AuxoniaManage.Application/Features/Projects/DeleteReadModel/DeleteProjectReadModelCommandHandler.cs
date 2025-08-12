using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;

namespace AuxoniaManage.Application.Features.Projects.DeleteReadModel;

public sealed class DeleteProjectReadModelCommandHandler : ICommandHandler<DeleteProjectReadModelCommand, DeleteProjectReadModelResponse>
{
    private readonly ILogger<DeleteProjectReadModelCommandHandler> _logger;
    private readonly IReadModelRepository _readModelRepository;

    public DeleteProjectReadModelCommandHandler
    (
        ILogger<DeleteProjectReadModelCommandHandler> logger,
        IReadModelRepository readModelRepository
    )
    {
        _logger = logger;
        _readModelRepository = readModelRepository;
    }

    public async Task<DeleteProjectReadModelResponse> Handle(DeleteProjectReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));

        _logger.LogInformation("Deleting project read model for ProjectId: {ProjectId}", request.ProjectId);

        var isDeleted = await _readModelRepository.DeleteProjectAsync(request.ProjectId, cancellationToken);

        if (!isDeleted)
        {
            _logger.LogWarning("Project read model not found or failed to delete for ProjectId: {ProjectId}", request.ProjectId);
        }
        else
        {
            _logger.LogInformation("Successfully deleted project read model for ProjectId: {ProjectId}", request.ProjectId);
        }

        return new DeleteProjectReadModelResponse(isDeleted, request.ProjectId);
    }
}