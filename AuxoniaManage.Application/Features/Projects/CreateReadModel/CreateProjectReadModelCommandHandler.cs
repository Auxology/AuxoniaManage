using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.ReadModels;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Projects.CreateReadModel;

public sealed class CreateProjectReadModelCommandHandler : ICommandHandler<CreateProjectReadModelCommand, CreateProjectReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public CreateProjectReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    public async Task<CreateProjectReadModelResponse> Handle(CreateProjectReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.ProjectId, nameof(request.ProjectId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        
        var existingReadModel = await _readModelRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        if (existingReadModel != null)
        {
            throw new ProjectReadModelAlreadyExistsException();
        }

        var newReadModel = new ProjectReadModel
        (
            projectId: request.ProjectId,
            name: request.Name,
            workspaceId: request.WorkspaceId,
            logoKey: request.LogoKey
        );
        
        var isSuccess = await _readModelRepository.AddProjectAsync(newReadModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProjectReadModelCreationFailedException();
        }
        
        return new CreateProjectReadModelResponse
        (
            Id: newReadModel.Id,
            ProjectId: newReadModel.ProjectId,
            Name: newReadModel.Name,
            WorkspaceId: newReadModel.WorkspaceId,
            LogoKey: newReadModel.LogoKey,
            CreatedAt: DateTime.UtcNow
        );
    }
}