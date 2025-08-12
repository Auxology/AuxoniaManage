using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Features.Membership;
using AuxoniaManage.Domain.Enums;

namespace AuxoniaManage.Application.Services;

public sealed class WorkspacePermissionService : IWorkspacePermissionService
{
    private readonly IMembershipRepository _membershipRepository;
    
    public WorkspacePermissionService(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    public async Task<bool> IsOwnerAsync(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetSpecificAsync(workspaceId, userId, cancellationToken);

        if (membership == null)
        {
            return false;
        }
        
        return membership.Role == WorkspaceRoles.Owner;
    }

    public async Task<bool> IsAdminAsync(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetSpecificAsync(workspaceId, userId, cancellationToken);

        if (membership == null)
        {
            return false;
        }
        
        return membership.Role == WorkspaceRoles.Admin;
    }

    public async Task<bool> IsMemberAsync(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetSpecificAsync(workspaceId, userId, cancellationToken);
        
        return membership != null;
    }

    public async Task<WorkspaceRoles> GetRoleAsync(Guid workspaceId, string userId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetSpecificAsync(workspaceId, userId, cancellationToken);

        if (membership == null)
        {
            throw new YouAreNotMemberOfWorkspaceException();
        }

        return membership.Role;
    }

    public async Task EnsureHierarchyAsync(Guid workspaceId, string userId, IReadOnlyList<string> userIds, CancellationToken cancellationToken)
    {
        var assignerMembership = await _membershipRepository.GetSpecificAsync(workspaceId, userId, cancellationToken);
        
        if (assignerMembership == null)
        {
            throw new YouAreNotMemberOfWorkspaceException();
        }
        
        if (assignerMembership.Role == WorkspaceRoles.Member)
        {
            throw new InsufficientPermissionsException();
        }
        
        var assigneeMemberships = await _membershipRepository.GetSpecificsAsync(workspaceId, userIds, cancellationToken);

        var expectedUserIds = userIds.ToHashSet();
        var foundUserIds = assigneeMemberships.Select(m => m.UserId).ToHashSet();
        var missingUserIds = expectedUserIds.Except(foundUserIds);
        
        if (missingUserIds.Any())
        {
            throw new OneOrMoreAssigneesNotMemberOfWorkspaceException();
        }
        
        var cannotAssignTo = assigneeMemberships
            .Where(m => !CanAssign(assignerMembership.Role, m.Role))
            .Select(m => m.UserId)
            .ToList();
        
        if (cannotAssignTo.Count > 0)
        {
            throw new CannotAssignRolesToHigherHierarchyException($"You cannot assign roles to the following users: {string.Join(", ", cannotAssignTo)}");
        }
    }
    
    private static bool CanAssign(WorkspaceRoles assignerRole, WorkspaceRoles assigneeRole)
    {
        return assignerRole switch
        {
            WorkspaceRoles.Owner => true,
            WorkspaceRoles.Admin => assigneeRole != WorkspaceRoles.Owner && assigneeRole != WorkspaceRoles.Admin,
            WorkspaceRoles.Member => false,
            _ => throw new ArgumentOutOfRangeException(nameof(assignerRole), assignerRole, "Invalid workspace role")
        };
    }
}