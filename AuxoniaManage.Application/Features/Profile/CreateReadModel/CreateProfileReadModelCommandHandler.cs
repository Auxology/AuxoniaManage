using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.ReadModels;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Profile.CreateReadModel;

public sealed class CreateProfileReadModelCommandHandler : ICommandHandler<CreateProfileReadModelCommand, CreateProfileReadModelResponse>
{
    private readonly IReadModelRepository _readModelRepository;
    
    public CreateProfileReadModelCommandHandler(IReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }
    
    
    public async Task<CreateProfileReadModelResponse> Handle(CreateProfileReadModelCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.ProfileId, nameof(request.ProfileId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.FullName, nameof(request.FullName));
        
        var readModel = await _readModelRepository.GetProfileAsync(request.UserId, cancellationToken);

        if (readModel != null)
        {
            throw new ProfileReadModelAlreadyExistsException();
        }

        var newReadModel = new ProfileReadModel
        (
            profileId: request.ProfileId,
            userId: request.UserId,
            email: request.Email,
            fullName: request.FullName,
            avatarKey: null
        );
        
        var isSuccess = await _readModelRepository.AddProfileAsync(newReadModel, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProfileReadModelCreationFailedException();
        }
        
        return new CreateProfileReadModelResponse
        (
            Id: newReadModel.Id,
            FullName: newReadModel.FullName,
            Email: newReadModel.Email,
            AvatarKey: newReadModel.AvatarKey,
            CreatedAt: DateTime.UtcNow
        );
    }
}