using System.Security.Claims;
using AuxoniaManage.Application.Features.Profile.Get;
using AuxoniaManage.Application.Features.Profile.Update;
using AuxoniaManage.Presentation.Dto.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuxoniaManage.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;
    
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new UpdateProfileCommand
        (
            UserId: userId,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Avatar: request.Avatar
        );
        
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
    
    [HttpGet("get")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetProfileQuery(UserId: userId);
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }
}