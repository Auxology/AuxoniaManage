using System.Security.Claims;
using AuxoniaManage.Application.Features.ProjectTask.Create;
using AuxoniaManage.Application.Features.ProjectTask.CreateTask;
using AuxoniaManage.Application.Features.ProjectTask.Delete;
using AuxoniaManage.Application.Features.ProjectTask.Edit;
using AuxoniaManage.Application.Features.ProjectTask.Get;
using AuxoniaManage.Presentation.Dto.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectTaskController : Controller
{
    private readonly IMediator _mediator;
    
    public ProjectTaskController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] CreateProjectTaskRequest request, [FromQuery] Guid workspaceId, [FromQuery] Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(); 
        }
        
        var command = new CreateTaskCommand
        (
            userId,
            workspaceId,
            projectId,
            request.AssigneeIds,
            request.Title,
            request.Description,
            request.DueDate,
            request.Priority,
            request.Status
        );
        
        var response = await _mediator.Send(command);

        return Ok(response);
    }
    
    [HttpGet("get")]
    [Authorize]
    public async Task<IActionResult> GetTask([FromQuery] Guid workspaceId, [FromQuery] Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var query = new GetTasksQuery
        (
            userId,
            workspaceId,
            projectId
        );
        
        var response = await _mediator.Send(query);

        return Ok(response);
    }
    
    [HttpPatch("edit")]
    [Authorize]
    public async Task<IActionResult> EditTask([FromBody] EditProjectTaskRequest request, 
        [FromQuery] Guid workspaceId, [FromQuery] Guid projectId, [FromQuery] Guid taskId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new EditTaskCommand
        (
            Id: taskId,
            UserId: userId,
            WorkspaceId: workspaceId,
            ProjectId: projectId,
            AssigneeIds: request.AssigneeIds ?? new List<string>(),
            Title: request.Title,
            Description: request.Description,
            DeadlineAt: request.DeadlineAt,
            Priority: request.Priority,
            Status: request.Status
        );
        
        var response = await _mediator.Send(command);

        return Ok(response);
    }
    
    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid workspaceId, [FromQuery] Guid projectId, [FromQuery] Guid taskId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        var command = new DeleteProjectTaskCommand
        (
            userId,
            taskId,
            projectId,
            workspaceId
        );
        
        await _mediator.Send(command);

        return NoContent();
    }
}