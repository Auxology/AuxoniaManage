using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.ReadModels;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Workspace.CreateReadModel;

public sealed class CreateWorkspaceReadModelCommandHandler : ICommandHandler<CreateWorkspaceReadModelCommand, CreateWorkspaceReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public CreateWorkspaceReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    public async Task<CreateWorkspaceReadModelResponse> Handle(CreateWorkspaceReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        
        var readModel = await _readModelRepository.GetWorkspaceAsync(request.WorkspaceId, cancellationToken);
        
        if (readModel != null)
        {
            throw new WorkspaceReadModelAlreadyExistsException();
        }
        
        var newReadModel = new WorkspaceReadModel
        (
            workspaceId: request.WorkspaceId,
            name: request.Name,
            logoKey: request.LogoKey
        );
        
        var isSuccess = await _readModelRepository.AddWorkspaceAsync(newReadModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new WorkspaceReadModelCreationFailedException();
        }
        
        return new CreateWorkspaceReadModelResponse
        (
            Id: newReadModel.Id,
            WorkspaceId: newReadModel.WorkspaceId,
            Name: newReadModel.Name,
            LogoKey: newReadModel.LogoKey,
            CreatedAt: DateTime.UtcNow
        );
    }
}