using AuxoniaManage.Application.Features.Projects.UpdateReadModel;
using AuxoniaManage.Domain.Events.Project;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Project;

public sealed class ProjectUpdatedReadModelConsumer : IConsumer<ProjectUpdatedEvent>
{
    private readonly ILogger<ProjectUpdatedReadModelConsumer> _logger;
    private readonly IMediator _mediator;

    public ProjectUpdatedReadModelConsumer
    (
        ILogger<ProjectUpdatedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProjectUpdatedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProjectUpdatedReadModelConsumer: Consuming message with ProjectId: {ProjectId}", message.Id);
        
        var command = new UpdateProjectReadModelCommand
        (
            ProjectId: message.Id,
            WorkspaceId: message.WorkspaceId,
            Name: message.Name,
            LogoKey: message.LogoKey
        );
        
        var response = await _mediator.Send(command, context.CancellationToken);
        
        _logger.LogInformation("ProjectUpdatedReadModelConsumer: Successfully updated read model for ProjectId: {ProjectId}, ReadModelId: {ReadModelId}", 
            message.Id, response.Id);
    }
}