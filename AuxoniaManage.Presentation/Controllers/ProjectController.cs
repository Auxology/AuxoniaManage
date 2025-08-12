using System.Security.Claims;
using AuxoniaManage.Application.Features.Projects.Create;
using AuxoniaManage.Application.Features.Projects.Delete;
using AuxoniaManage.Application.Features.Projects.Get;
using AuxoniaManage.Application.Features.Projects.GetMany;
using AuxoniaManage.Application.Features.Projects.Update;
using AuxoniaManage.Presentation.Dto.Project;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
    private readonly IMediator _mediator;
    
    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateProject([FromForm] CreateProjectRequest request, [FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new CreateProjectCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId,
            Name: request.Name,
            Logo: request.Logo
        );
        
        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectRequest request,
        [FromQuery] Guid workspaceId, [FromQuery] Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new UpdateProjectCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId,
            Id: projectId,
            Name: request.Name,
            Logo: request.Logo
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpGet("get")]
    [Authorize]
    public async Task<IActionResult> GetProject([FromQuery] Guid workspaceId, [FromQuery] Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new GetProjectQuery
        (
            Id: projectId,
            UserId: userId,
            WorkspaceId: workspaceId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpGet("get-all")]
    [Authorize]
    public async Task<IActionResult> GetAllProjects([FromQuery] Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new GetManyProjectsQuery
        (
            UserId: userId,
            WorkspaceId: workspaceId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteProject([FromQuery] Guid workspaceId, [FromQuery] Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new DeleteProjectCommand
        (
            UserId: userId,
            WorkspaceId: workspaceId,
            ProjectId: projectId
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
}