using System.Security.Claims;
using AuxoniaManage.Application.Features.Membership.GetWorkspaceMemberships;
using AuxoniaManage.Application.Features.Membership.GetWorkspaces;
using AuxoniaManage.Application.Features.Membership.KickMember;
using AuxoniaManage.Application.Features.Membership.MakeAdmin;
using AuxoniaManage.Application.Features.Membership.MakeMember;
using AuxoniaManage.Application.Features.Membership.TransferOwnership;
using AuxoniaManage.Application.Features.Membership.Leave;
using AuxoniaManage.Application.Features.Onboarding.AcceptInvitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public MembershipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-workspaces")]
    [Authorize]
    public async Task<IActionResult> GetWorkspaces()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var query = new GetWorkspacesQuery(userId);
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }

    [HttpGet("get-members")]
    [Authorize]
    public async Task<IActionResult> GetMembers([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var query = new GetWorkspaceMembershipsQuery(WorkspaceId: workspaceId, UserId: userId);
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }
    
    [HttpPost("kick-member")]
    [Authorize]
    public async Task<IActionResult> KickMember([FromQuery] Guid workspaceId, [FromQuery] string memberId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new KickMemberCommand
        (
            WorkspaceId: workspaceId,
            UserId: userId,
            MemberId: memberId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [HttpPatch("make-admin")]
    [Authorize]
    public async Task<IActionResult> MakeAdmin([FromQuery] Guid workspaceId, [FromQuery] string newAdminId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new MakeAdminCommand
        (
            WorkspaceId: workspaceId,
            UserId: userId,
            NewAdminId: newAdminId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [HttpPatch("make-member")]
    [Authorize]
    public async Task<IActionResult> MakeMember([FromQuery] Guid workspaceId, [FromQuery] string newMemberId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new MakeMemberCommand
        (
            WorkspaceId: workspaceId,
            UserId: userId,
            NewMemberId: newMemberId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [HttpPatch("transfer-ownership")]
    [Authorize]
    public async Task<IActionResult> TransferOwnership([FromQuery] Guid workspaceId, [FromQuery] string newOwnerId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new TransferOwnershipCommand
        (
            UserId: userId,
            NewOwnerId: newOwnerId,
            WorkspaceId: workspaceId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [HttpPost("leave")]
    [Authorize]
    public async Task<IActionResult> LeaveWorkspace([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new LeaveWorkspaceCommand
        (
            WorkspaceId: workspaceId,
            UserId: userId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpPost("accept-invite")]
    [Authorize]
    public async Task<IActionResult> AcceptInvite([FromQuery] Guid workspaceId, [FromQuery] string invitationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new AcceptInvitationCommand
        (
            WorkspaceId: workspaceId,
            UserId: userId,
            InvitationToken: invitationToken
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
}