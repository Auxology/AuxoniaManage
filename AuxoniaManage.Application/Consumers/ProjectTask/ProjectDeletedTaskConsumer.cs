using AuxoniaManage.Application.Features.ProjectTask;
using AuxoniaManage.Domain.Events.Project;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.ProjectTask;

public sealed class ProjectDeletedTaskConsumer : IConsumer<ProjectDeletedEvent>
{
    private readonly ILogger<ProjectDeletedTaskConsumer> _logger;
    private readonly IProjectTaskRepository _projectTaskRepository;

    public ProjectDeletedTaskConsumer
    (
        ILogger<ProjectDeletedTaskConsumer> logger,
        IProjectTaskRepository projectTaskRepository
    )
    
    {
        _logger = logger;
        _projectTaskRepository = projectTaskRepository;
    }


    public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("ProjectDeletedTaskConsumer: Consuming message for project {ProjectId}", message.Id);
        
        var projectTasks = await _projectTaskRepository.GetAllAsync(message.Id, context.CancellationToken);

        try
        {
            var affected = await _projectTaskRepository.DeleteRangeAsync(projectTasks, context.CancellationToken);

            _logger.LogInformation(
                "ProjectDeletedTaskConsumer: Successfully deleted {Count} tasks for project {ProjectId}", affected,
                message.Id);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProjectDeletedTaskConsumer: Error deleting tasks for project {ProjectId}", message.Id);
            throw;
        }
    }
}