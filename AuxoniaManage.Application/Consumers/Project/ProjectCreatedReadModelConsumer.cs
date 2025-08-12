using AuxoniaManage.Application.Features.Projects.CreateReadModel;
using AuxoniaManage.Domain.Events.Project;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.Project;

public sealed class ProjectCreatedReadModelConsumer : IConsumer<ProjectCreatedEvent>
{
    private readonly ILogger<ProjectCreatedReadModelConsumer> _logger;
    private readonly IMediator _mediator;

    public ProjectCreatedReadModelConsumer
    (
        ILogger<ProjectCreatedReadModelConsumer> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProjectCreatedReadModelConsumer: Consuming message with ProjectId: {ProjectId}", message.Id);
        
        var command = new CreateProjectReadModelCommand
        (
            ProjectId: message.Id,
            WorkspaceId: message.WorkspaceId,
            Name: message.Name,
            LogoKey: message.LogoKey
        );
        
        var response = await _mediator.Send(command, context.CancellationToken);
        
        _logger.LogInformation("ProjectCreatedReadModelConsumer: Successfully created read model for ProjectId: {ProjectId}, ReadModelId: {ReadModelId}", 
            message.Id, response.Id);
    }
}