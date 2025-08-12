using Ardalis.GuardClauses;
using AuxoniaManage.Application.Features.Membership.Create;
using AuxoniaManage.Application.Features.Workspace.Create;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MediatR;

namespace AuxoniaManage.Application.Features.Onboarding.CreateWorkspaceWithMembership;

public sealed class CreateWorkspaceWithMembershipCommandHandler : ICommandHandler<CreateWorkspaceWithMembershipCommand, CreateWorkspaceWithMembershipResponse>
{
    private readonly IMediator _mediator;
    
    public CreateWorkspaceWithMembershipCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<CreateWorkspaceWithMembershipResponse> Handle(CreateWorkspaceWithMembershipCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        Guard.Against.NullOrEmpty(request.Description, nameof(request.Description));
        
        var createWorkspaceCommand = new CreateWorkspaceCommand
        (
            OwnerId: request.UserId,
            Name: request.Name,
            Description: request.Description,
            Logo: request.Logo
        );
        
        var workspaceResponse = await _mediator.Send(createWorkspaceCommand, cancellationToken);
        
        var createMembershipCommand = new CreateMembershipCommand
        (
            UserId: request.UserId,
            WorkspaceId: workspaceResponse.Id,
            Role: WorkspaceRoles.Owner
        );
        
        var membershipResponse = await _mediator.Send(createMembershipCommand, cancellationToken);
        
        return new CreateWorkspaceWithMembershipResponse
        (
            WorkspaceId: workspaceResponse.Id,
            OwnerId: request.UserId,
            MembershipId: membershipResponse.Id,
            Name: workspaceResponse.Name,
            Description: workspaceResponse.Description,
            CreatedAt: workspaceResponse.CreatedAt,
            LogoKey: workspaceResponse.LogoKey
        );
    }
}