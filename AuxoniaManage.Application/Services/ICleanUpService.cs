namespace AuxoniaManage.Application.Services;

public interface ICleanUpService
{
    Task<bool> CleanUpProjectTasks(Guid workspaceId, string userId, CancellationToken cancellationToken);
}