using System.Security.Claims;
using AuxoniaManage.Application.Features.Onboarding.CreateWorkspaceWithMembership;
using AuxoniaManage.Application.Features.Workspace.Delete;
using AuxoniaManage.Application.Features.Workspace.Get;
using AuxoniaManage.Application.Features.Workspace.RotateInvitation;
using AuxoniaManage.Application.Features.Workspace.Update;
using AuxoniaManage.Presentation.Dto.Workspace;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public WorkspaceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateWorkspace([FromForm] CreateWorkspaceRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new CreateWorkspaceWithMembershipCommand
        (
            UserId: userId,
            Name: request.Name,
            Description: request.Description,
            Logo: request.Logo
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpGet("get")]
    [Authorize]
    public async Task<IActionResult> GetWorkspace([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        var query = new GetWorkspaceQuery(userId, workspaceId);
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }
    
    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> UpdateWorkspace([FromForm] UpdateWorkspaceRequest request, [FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        
        var command = new UpdateWorkspaceCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId,
            Name: request.Name,
            Description: request.Description,
            Logo: request.Logo
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpPatch("rotate-invitation")]
    [Authorize]
    public async Task<IActionResult> RotateInvitationCode([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        
        var command = new RotateInvitationCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteWorkspace([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        
        var command = new DeleteWorkspaceCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
}