using AuxoniaManage.Application.Features.ProjectTask;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Events.Membership;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuxoniaManage.Application.Consumers.ProjectTask;

public sealed class MembershipDeleteTaskConsumer :
    IConsumer<MembershipDeletedEvent>,
    IConsumer<MemberKickedEvent>
{
    private readonly ILogger<MembershipDeleteTaskConsumer> _logger;
    private readonly ICleanUpService _cleanUpService;

    public MembershipDeleteTaskConsumer
    (
        ILogger<MembershipDeleteTaskConsumer> logger,
        ICleanUpService cleanUpService
    )
    {
        _logger = logger;
        _cleanUpService = cleanUpService;
    }

    public Task Consume(ConsumeContext<MembershipDeletedEvent> context) =>
        HandleCleanupAsync(context, context.Message.WorkspaceId, context.Message.UserId);

    public Task Consume(ConsumeContext<MemberKickedEvent> context) =>
        HandleCleanupAsync(context, context.Message.WorkspaceId, context.Message.KickedMemberId);

    private async Task HandleCleanupAsync<T>(ConsumeContext<T> context, Guid workspaceId, string userId)
        where T : class
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["WorkspaceId"] = workspaceId,
            ["UserId"] = userId,
            ["CorrelationId"] = context.CorrelationId
        });

        var ok = await _cleanUpService.CleanUpProjectTasks(workspaceId, userId, context.CancellationToken);

        if (ok)
        {
            _logger.LogInformation("Cleaned up project tasks for user {UserId} in workspace {WorkspaceId}.", userId, workspaceId);
            return;
        }

        _logger.LogError("Cleanup failed for user {UserId} in workspace {WorkspaceId}. Throwing to trigger retry.", userId, workspaceId);
        
        throw new InvalidOperationException($"Cleanup failed for user {userId} in workspace {workspaceId}");
    }
}
