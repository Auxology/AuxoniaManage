namespace AuxoniaManage.Application.Exceptions;

public sealed class UserDoesNotHavePermissionException : Exception
{
    public UserDoesNotHavePermissionException() : base("User does not have permission to perform this action.")
    {
    }
    
    public UserDoesNotHavePermissionException(string message) : base(message)
    {
    }

    public UserDoesNotHavePermissionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class FailedToCreateProjectException : Exception
{
    public FailedToCreateProjectException() : base("Failed to create project, please try again later.")
    {
    }
    
    public FailedToCreateProjectException(string message) : base(message)
    {
    }

    public FailedToCreateProjectException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException() : base("Project not found.")
    {
    }
    
    public ProjectNotFoundException(string message) : base(message)
    {
    }

    public ProjectNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectReadModelAlreadyExistsException : Exception
{
    public ProjectReadModelAlreadyExistsException() : base("Project read model already exists.")
    {
    }
    
    public ProjectReadModelAlreadyExistsException(string message) : base(message)
    {
    }

    public ProjectReadModelAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectReadModelCreationFailedException : Exception
{
    public ProjectReadModelCreationFailedException() : base("Failed to create project read model.")
    {
    }
    
    public ProjectReadModelCreationFailedException(string message) : base(message)
    {
    }

    public ProjectReadModelCreationFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class FailedToUpdateProjectException : Exception
{
    public FailedToUpdateProjectException() : base("Failed to update project, please try again later.")
    {
    }
    
    public FailedToUpdateProjectException(string message) : base(message)
    {
    }

    public FailedToUpdateProjectException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectReadModelNotFoundException : Exception
{
    public ProjectReadModelNotFoundException() : base("Project read model not found.")
    {
    }
    
    public ProjectReadModelNotFoundException(string message) : base(message)
    {
    }

    public ProjectReadModelNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectReadModelUpdateFailedException : Exception
{
    public ProjectReadModelUpdateFailedException() : base("Failed to update project read model.")
    {
    }
    
    public ProjectReadModelUpdateFailedException(string message) : base(message)
    {
    }

    public ProjectReadModelUpdateFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public sealed class ProjectDeletionFailedException : Exception
{
    public ProjectDeletionFailedException() : base("Failed to delete project, please try again later.")
    {
    }
    
    public ProjectDeletionFailedException(string message) : base(message)
    {
    }

    public ProjectDeletionFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}