using AuxoniaManage.Application.Features.Projects.DeleteReadModel;
using AuxoniaManage.Domain.Events.Project;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Project;

public sealed class ProjectDeletedReadModelConsumer : IConsumer<ProjectDeletedEvent>
{
    private readonly ILogger<ProjectDeletedReadModelConsumer> _logger;
    private readonly IMediator _mediator;

    public ProjectDeletedReadModelConsumer
    (
        ILogger<ProjectDeletedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProjectDeletedReadModelConsumer: Consuming message with ProjectId: {ProjectId}", message.Id);
        
        var command = new DeleteProjectReadModelCommand
        (
            ProjectId: message.Id
        );
        
        var response = await _mediator.Send(command, context.CancellationToken);
        
        if (response.IsDeleted)
        {
            _logger.LogInformation("ProjectDeletedReadModelConsumer: Successfully deleted read model for ProjectId: {ProjectId}", message.Id);
        }
        else
        {
            _logger.LogWarning("ProjectDeletedReadModelConsumer: Failed to delete read model or project not found for ProjectId: {ProjectId}", message.Id);
        }
    }
}