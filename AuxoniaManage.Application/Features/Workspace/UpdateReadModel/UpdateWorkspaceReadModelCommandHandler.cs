using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Workspace.UpdateReadModel;

public sealed class UpdateWorkspaceReadModelCommandHandler : ICommandHandler<UpdateWorkspaceReadModelCommand, UpdateWorkspaceReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public UpdateWorkspaceReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    public async Task<UpdateWorkspaceReadModelResponse> Handle(UpdateWorkspaceReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        
        var readModel = await _readModelRepository.GetWorkspaceAsync(request.WorkspaceId, cancellationToken);
        
        if (readModel == null)
        {
            throw new WorkspaceReadModelNotFoundException(request.WorkspaceId);
        }
        
        readModel.UpdateReadModel(request.Name, request.LogoKey);
        
        var isSuccess = await _readModelRepository.UpdateWorkspaceAsync(readModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new WorkspaceReadModelUpdateFailedException();
        }
        
        return new UpdateWorkspaceReadModelResponse
        (
            Id: readModel.Id,
            Name: readModel.Name,
            LogoKey: readModel.LogoKey,
            UpdatedAt: DateTime.UtcNow
        );
    }
}