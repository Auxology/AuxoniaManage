using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;

namespace AuxoniaManage.Application.Features.Projects.DeleteManyReadModels;

public sealed class DeleteProjectsReadModelCommandHandler : ICommandHandler<DeleteProjectsReadModelCommand, DeleteProjectsReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;

    public DeleteProjectsReadModelCommandHandler
    (
        IReadModelRepository readModelRepository
    )
    {
        _readModelRepository = readModelRepository;
    }

    public async Task<DeleteProjectsReadModelResponse> Handle(DeleteProjectsReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.ProjectIds, nameof(request.ProjectIds));


        var projectReadModels = await _readModelRepository.GetProjectsAsync(request.ProjectIds, cancellationToken);

        if (projectReadModels.Count == 0)
        {
            return new DeleteProjectsReadModelResponse(
                [],
                [],
                DateTime.UtcNow
            );
        }
        
        var projectReadModelIds = projectReadModels.Select(p => p.Id).ToList();
        
        var isSuccess = await _readModelRepository.DeleteProjectsAsync(projectReadModels, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProjectDeletionFailedException();
        }

        return new DeleteProjectsReadModelResponse
        (
            projectReadModelIds,
            request.ProjectIds,
            DateTime.UtcNow
        );
    }
}