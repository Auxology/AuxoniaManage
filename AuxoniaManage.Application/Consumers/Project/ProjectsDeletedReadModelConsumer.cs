using AuxoniaManage.Application.Features.Projects.DeleteManyReadModels;
using AuxoniaManage.Domain.Events.Project;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Project;

public sealed class ProjectsDeletedReadModelConsumer : IConsumer<ProjectsDeletedEvent>
{
    private readonly ILogger<ProjectUpdatedReadModelConsumer> _logger;
    private readonly IMediator _mediator;

    public ProjectsDeletedReadModelConsumer
    (
        ILogger<ProjectUpdatedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProjectsDeletedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProjectsDeletedReadModelConsumer: Consuming message with WorkspaceId: {WorkspaceId}", message.WorkspaceId);
        
        var command = new DeleteProjectsReadModelCommand
        (
            ProjectIds: message.Ids
        );
        
        await _mediator.Send(command, context.CancellationToken);
        
        _logger.LogInformation("ProjectsDeletedReadModelConsumer: Successfully deleted read models for WorkspaceId: {WorkspaceId}", 
            message.WorkspaceId);
    }
}