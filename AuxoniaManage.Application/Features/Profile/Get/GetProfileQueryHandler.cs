using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Profile.Get;

public sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, GetProfileResponse>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IStorageService _storageService;

    public GetProfileQueryHandler
    (
        IProfileRepository profileRepository,
        IStorageService storageService
    )

    {
        _profileRepository = profileRepository;
        _storageService = storageService;
    }

    public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (profile == null)
        {
            throw new ProfileNotFoundException();
        }
        
        var avatarUrl = string.IsNullOrEmpty(profile.AvatarKey)
            ? null
            : await _storageService.ConstructUrlAsync(profile.AvatarKey, cancellationToken);

        return new GetProfileResponse
        (
            Id: profile.Id,
            UserId: profile.UserId,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            Email: profile.Email,
            CreatedAt: profile.CreatedAt,
            UpdatedAt: profile.UpdatedAt,
            AvatarUrl: avatarUrl
        );
    }
}