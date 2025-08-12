using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Profile.UpdateReadModel;

public sealed class UpdateProfileReadModelCommandHandler : ICommandHandler<UpdateProfileReadModelCommand, UpdateProfileReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public UpdateProfileReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    public async Task<UpdateProfileReadModelResponse> Handle(UpdateProfileReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.ProfileId, nameof(request.ProfileId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.FullName, nameof(request.FullName));
        
        var readModel = await _readModelRepository.GetProfileAsync(request.UserId, cancellationToken);
        
        if (readModel == null)
        {
            throw new ProfileReadModelNotFoundException();
        }
        
        readModel.UpdateReadModel(request.FullName, request.AvatarKey);
        
        var isSuccess = await _readModelRepository.UpdateProfileAsync(readModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProfileReadModelUpdateFailedException();
        }
        
        return new UpdateProfileReadModelResponse
        (
            Id: readModel.Id,
            FullName: readModel.FullName,
            AvatarKey: readModel.AvatarKey,
            UpdatedAt: DateTime.UtcNow
        );
    }
}