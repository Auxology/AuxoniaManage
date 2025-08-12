using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;

namespace AuxoniaManage.Application.Features.Membership.GetWorkspaceMemberships;

public sealed class GetWorkspaceMembershipsQueryHandler : IQueryHandler<GetWorkspaceMembershipsQuery, GetWorkspaceMembershipsResponse>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IReadModelRepository _readModelRepository;
    private readonly IStorageService _storageService;

    public GetWorkspaceMembershipsQueryHandler
    (
        IMembershipRepository membershipRepository,
        IReadModelRepository readModelRepository,
        IStorageService storageService
    )
    {
        _membershipRepository = membershipRepository;
        _readModelRepository = readModelRepository;
        _storageService = storageService;
    }

    public async Task<GetWorkspaceMembershipsResponse> Handle(GetWorkspaceMembershipsQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        
        var userMembership = await _membershipRepository.GetSpecificAsync(
            request.WorkspaceId, 
            request.UserId, 
            cancellationToken);
        
        if (userMembership == null)
        {
            throw new MembershipNotFoundException(request.WorkspaceId, request.UserId);
        }

        var memberships = await _membershipRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);
        if (memberships.Count == 0)
        {
            return new GetWorkspaceMembershipsResponse([]);
        }

        var profileReadModels = await _readModelRepository.GetProfileByUserId(
            memberships.Select(m => m.UserId).ToArray(),
            cancellationToken);

        if (profileReadModels.Count == 0)
        {
            return new GetWorkspaceMembershipsResponse([]);
        }

        var membershipTask = memberships.Select(async m =>
        {
            var profile = profileReadModels.FirstOrDefault(p => p.UserId == m.UserId);
            return new MembershipDto
            (
                m.Id,
                m.UserId,
                profile?.FullName ?? string.Empty,
                profile?.AvatarKey != null
                    ? await _storageService.ConstructUrlAsync(profile.AvatarKey, cancellationToken)
                    : null
                ,
                m.WorkspaceId,
                m.Role.ToString(),
                m.JoinedAt,
                m.UpdatedAt
            );
        });
        
        var membershipDtos = await Task.WhenAll(membershipTask);
        
        
        return new GetWorkspaceMembershipsResponse(membershipDtos.ToList());
    }
}