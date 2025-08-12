using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Profile;
using AuxoniaManage.Domain.Events.Storage;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;
using MediatR;

namespace AuxoniaManage.Application.Features.Profile.Update;

public sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, UpdateProfileResponse>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IStorageService _storageService;
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateProfileCommandHandler
    (
        IProfileRepository profileRepository,
        IStorageService storageService,
        IMediator mediator,
        IPublishEndpoint publishEndpoint
    )
    
    {
        _profileRepository = profileRepository;
        _storageService = storageService;
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UpdateProfileResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (profile == null)
        {
            throw new ProfileNotFoundException();
        }
        
        var firstName = request.FirstName ?? profile.FirstName;
        var lastName = request.LastName ?? profile.LastName;
        var oldKey = profile.AvatarKey;
        var newKey = oldKey;
        
        if (request.Avatar != null)
        {
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                var objectRemovedEvent = new ObjectRemovedEvent(oldKey);
            
                await _publishEndpoint.Publish(objectRemovedEvent, cancellationToken);
            }
            
            newKey = await _storageService.PutObjectAsync
            (
                file: request.Avatar.OpenReadStream(),
                sender: Senders.Profile,
                request.Avatar.FileName,
                request.Avatar.ContentType,
                request.Avatar.Length,
                cancellationToken
            );
        }
        
        profile.UpdateProfile
        (
            firstName: firstName,
            lastName: lastName,
            avatarKey: newKey,
            timeStamp: DateTime.UtcNow
        );
        
        var isUpdated = await _profileRepository.UpdateAsync(profile, cancellationToken);
        
        if (!isUpdated)
        {
            throw new ProfileUpdateFailedException();
        }
        
        var profileUpdatedEvent = new ProfileUpdatedEvent
        (
            Id: profile.Id,
            UserId: profile.UserId,
            FullName: $"{profile.FirstName} {profile.LastName}",
            AvatarKey: profile.AvatarKey
        );
        
        await _publishEndpoint.Publish(profileUpdatedEvent, cancellationToken);
        
        return new UpdateProfileResponse
        (
            Id: profile.Id,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            UpdatedAt: profile.UpdatedAt,
            AvatarKey: profile.AvatarKey
        );
    }
}