using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Projects.UpdateReadModel;

public sealed class UpdateProjectReadModelCommandHandler : ICommandHandler<UpdateProjectReadModelCommand, UpdateProjectReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public UpdateProjectReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    public async Task<UpdateProjectReadModelResponse> Handle(UpdateProjectReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        
        var existingReadModel = await _readModelRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        if (existingReadModel == null)
        {
            throw new ProjectReadModelNotFoundException();
        }

        existingReadModel.UpdateProject
        (
            name: request.Name,
            logoKey: request.LogoKey
        );
        
        var isSuccess = await _readModelRepository.UpdateProjectAsync(existingReadModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProjectReadModelUpdateFailedException();
        }
        
        return new UpdateProjectReadModelResponse
        (
            Id: existingReadModel.Id,
            ProjectId: existingReadModel.ProjectId,
            Name: existingReadModel.Name,
            WorkspaceId: existingReadModel.WorkspaceId,
            LogoKey: existingReadModel.LogoKey,
            UpdatedAt: DateTime.UtcNow
        );
    }
}