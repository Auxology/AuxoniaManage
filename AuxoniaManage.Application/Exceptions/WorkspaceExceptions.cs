namespace AuxoniaManage.Application.Exceptions;


public sealed class WorkspaceCreationFailedException : Exception
{
    public WorkspaceCreationFailedException() 
        : base("Workspace creation failed due to invalid data or system error.")
    {
    }

    public WorkspaceCreationFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceCreationFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceNotFoundException : Exception
{
    public WorkspaceNotFoundException() 
        : base("Workspace not found.")
    {
    }

    public WorkspaceNotFoundException(Guid workspaceId) 
        : base($"Workspace with ID '{workspaceId}' not found.")
    {
    }

    public WorkspaceNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceUpdateFailedException : Exception
{
    public WorkspaceUpdateFailedException() 
        : base("Workspace update failed due to invalid data or system error.")
    {
    }

    public WorkspaceUpdateFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceUpdateFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceDeletionFailedException : Exception
{
    public WorkspaceDeletionFailedException() 
        : base("Workspace deletion failed due to system error.")
    {
    }

    public WorkspaceDeletionFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceDeletionFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceAccessDeniedException : Exception
{
    public WorkspaceAccessDeniedException() 
        : base("Access to workspace is denied.")
    {
    }

    public WorkspaceAccessDeniedException(Guid workspaceId) 
        : base($"Access to workspace '{workspaceId}' is denied.")
    {
    }

    public WorkspaceAccessDeniedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceNameAlreadyExistsException : Exception
{
    public WorkspaceNameAlreadyExistsException() 
        : base("A workspace with this name already exists.")
    {
    }

    public WorkspaceNameAlreadyExistsException(string workspaceName) 
        : base($"A workspace with the name '{workspaceName}' already exists.")
    {
    }

    public WorkspaceNameAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceMemberLimitExceededException : Exception
{
    public WorkspaceMemberLimitExceededException() 
        : base("Workspace member limit has been exceeded.")
    {
    }

    public WorkspaceMemberLimitExceededException(int limit) 
        : base($"Workspace member limit of {limit} has been exceeded.")
    {
    }

    public WorkspaceMemberLimitExceededException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceOwnershipTransferFailedException : Exception
{
    public WorkspaceOwnershipTransferFailedException() 
        : base("Workspace ownership transfer failed.")
    {
    }

    public WorkspaceOwnershipTransferFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceOwnershipTransferFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceReadModelAlreadyExistsException : Exception
{
    public WorkspaceReadModelAlreadyExistsException() 
        : base("Workspace read model already exists.")
    {
    }

    public WorkspaceReadModelAlreadyExistsException(Guid workspaceId) 
        : base($"Workspace read model for workspace '{workspaceId}' already exists.")
    {
    }

    public WorkspaceReadModelAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceReadModelCreationFailedException : Exception
{
    public WorkspaceReadModelCreationFailedException() 
        : base("Failed to create workspace read model.")
    {
    }

    public WorkspaceReadModelCreationFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceReadModelCreationFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceReadModelNotFoundException : Exception
{
    public WorkspaceReadModelNotFoundException() 
        : base("Workspace read model not found.")
    {
    }

    public WorkspaceReadModelNotFoundException(Guid workspaceId) 
        : base($"Workspace read model for workspace '{workspaceId}' not found.")
    {
    }

    public WorkspaceReadModelNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceReadModelUpdateFailedException : Exception
{
    public WorkspaceReadModelUpdateFailedException() 
        : base("Workspace read model update failed due to invalid data or system error.")
    {
    }

    public WorkspaceReadModelUpdateFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceReadModelUpdateFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class WorkspaceReadModelDeletionFailedException : Exception
{
    public WorkspaceReadModelDeletionFailedException() 
        : base("Workspace read model deletion failed due to system error.")
    {
    }

    public WorkspaceReadModelDeletionFailedException(string message) 
        : base(message)
    {
    }

    public WorkspaceReadModelDeletionFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class OnlyWorkspaceOwnerCanUpdateException : Exception
{
    public OnlyWorkspaceOwnerCanUpdateException() 
        : base("Only the workspace owner can update the workspace.")
    {
    }

    public OnlyWorkspaceOwnerCanUpdateException(Guid workspaceId) 
        : base($"Only the owner of workspace '{workspaceId}' can perform this action.")
    {
    }

    public OnlyWorkspaceOwnerCanUpdateException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class UserIsNotMemberException : Exception
{
    public UserIsNotMemberException() 
        : base("User is not a member of the workspace.")
    {
    }

    public UserIsNotMemberException(string userId, Guid workspaceId) 
        : base($"User '{userId}' is not a member of workspace '{workspaceId}'.")
    {
    }

    public UserIsNotMemberException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class OnlyOwnerCanRotateInvitationException : Exception
{
    public OnlyOwnerCanRotateInvitationException() 
        : base("Only the workspace owner can rotate the invitation code.")
    {
    }

    public OnlyOwnerCanRotateInvitationException(Guid workspaceId) 
        : base($"Only the owner of workspace '{workspaceId}' can rotate the invitation code.")
    {
    }

    public OnlyOwnerCanRotateInvitationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class InvalidInvitationTokenException : Exception
{
    public InvalidInvitationTokenException() 
        : base("The provided invitation token is invalid.")
    {
    }

    public InvalidInvitationTokenException(string token) 
        : base($"The provided invitation token '{token}' is invalid.")
    {
    }

    public InvalidInvitationTokenException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class OnlyOwnerCanDeleteWorkspaceException : Exception
{
    public OnlyOwnerCanDeleteWorkspaceException() 
        : base("Only the workspace owner can delete the workspace.")
    {
    }

    public OnlyOwnerCanDeleteWorkspaceException(Guid workspaceId) 
        : base($"Only the owner of workspace '{workspaceId}' can delete it.")
    {
    }

    public OnlyOwnerCanDeleteWorkspaceException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FailedToDeleteWorkspaceException : Exception
{
    public FailedToDeleteWorkspaceException() 
        : base("Failed to delete the workspace, please try again later.")
    {
    }

    public FailedToDeleteWorkspaceException(string message) 
        : base(message)
    {
    }

    public FailedToDeleteWorkspaceException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}