using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Events.Profile;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using MassTransit;

namespace AuxoniaManage.Application.Features.Profile.Create;

public sealed class CreateProfileCommandHandler : ICommandHandler<CreateProfileCommand, CreateProfileResponse>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateProfileCommandHandler
    (
        IProfileRepository profileRepository,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _profileRepository = profileRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<CreateProfileResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.FirstName, nameof(request.FirstName));
        Guard.Against.NullOrEmpty(request.LastName, nameof(request.LastName));
        
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (profile != null)
        {
            throw new ProfileAlreadyExistsException();
        }
        
        var newProfile = new Domain.Entities.UserProfile
        (
            userId: request.UserId,
            firstName: request.FirstName,
            email: request.Email,
            lastName: request.LastName,
            timeStamp: DateTime.UtcNow
        );
        
        var isSuccess = await _profileRepository.AddAsync(newProfile, cancellationToken);
        
        if (!isSuccess)
        {
            throw new ProfileCreationFailedException();
        }

        var profileCreatedEvent = new ProfileCreatedEvent
        (
            Id: newProfile.Id,
            UserId: newProfile.UserId,
            FullName: $"{newProfile.FirstName} {newProfile.LastName}",
            Email: newProfile.Email
        );
        
        await _publishEndpoint.Publish(profileCreatedEvent, cancellationToken);
        
        return new CreateProfileResponse
        (
            Id: newProfile.Id,
            FirstName: newProfile.FirstName,
            LastName: newProfile.LastName,
            CreatedAt: newProfile.CreatedAt
        );
    }
}