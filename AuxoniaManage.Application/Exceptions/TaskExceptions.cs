namespace AuxoniaManage.Application.Exceptions;

public sealed class CannotAssignTaskToSelfException : Exception
{
    public CannotAssignTaskToSelfException()
        : base("You cannot assign a task to yourself.")
    {
    }
}

public sealed class InvalidProjectIdException : Exception
{
    public InvalidProjectIdException()
        : base("The provided project ID is invalid or does not exist.")
    {
    }
}

public sealed class FailedToCreateTaskException : Exception
{
    public FailedToCreateTaskException()
        : base("Failed to create the task. Please try again later.")
    {
    }
}

public sealed class CouldNotFindWorkspaceException : Exception
{
    public CouldNotFindWorkspaceException()
        : base("Could not find the workspace.")
    {
    }
}

public sealed class CouldNotFindProjectException : Exception
{
    public CouldNotFindProjectException()
        : base("Could not find the project.")
    {
    }
}


public sealed class TaskNotFoundException : Exception
{
    public TaskNotFoundException()
        : base($"Task could not be found.")
    {
    }
}

public sealed class LackOfPermissionException: Exception
{
    public LackOfPermissionException()
        : base("You do not have permission to perform this action.")
    {
    }
}

public sealed class FailedToEditTaskException : Exception
{
    public FailedToEditTaskException()
        : base("Failed to edit the task. Please try again later.")
    {
    }
}

public sealed class CouldNotDeleteTaskException : Exception
{
    public CouldNotDeleteTaskException()
        : base("Could not delete the task, please try again later.")
    {
    }
}