using AuxoniaManage.Domain.Enums.Tasks;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace AuxoniaManage.Domain.Entities;

public sealed class ProjectTask
{
    public Guid Id { get; private set; }
    
    public Guid ProjectId { get; private set; }
    
    public string AssignedById { get; private set; }
    
    public IReadOnlyList<string> AssigneeIds { get; private set; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime? DeadlineAt { get; private set; }
    
    public ProjectTaskStatus Status { get; private set; }
    
    public ProjectTaskPriority Priority { get; private set; }
    
    private ProjectTask()
    {
        // Required for EF Core
    }

    public ProjectTask 
    (
        Guid projectId, string assignedById, IReadOnlyList<string> assigneeIds, string title,
        string description, DateTime timeStamp, DateTime? deadlineAt,
        ProjectTaskStatus status, ProjectTaskPriority priority
    )

    {
        ProjectId = projectId;
        AssignedById = assignedById;
        AssigneeIds = assigneeIds;
        Title = title;
        Description = description;
        CreatedAt = timeStamp;
        UpdatedAt = timeStamp;
        DeadlineAt = deadlineAt;
        Status = status;
        Priority = priority;
    }
    
    public void UpdateProjectTask
    (
        IReadOnlyList<string> assigneeIds, string title, string description, DateTime? deadlineAt,
        ProjectTaskStatus status, ProjectTaskPriority priority, DateTime timeStamp
    )
    {
        AssigneeIds = assigneeIds;
        Title = title;
        Description = description;
        DeadlineAt = deadlineAt;
        Status = status;
        Priority = priority;
        UpdatedAt = timeStamp;
    }
}