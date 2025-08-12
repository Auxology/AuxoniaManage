namespace AuxoniaManage.Application.Exceptions;

public sealed class MembershipAlreadyExistsException : Exception
{
    public MembershipAlreadyExistsException() 
        : base("Membership already exists for this user in the workspace.")
    {
    }

    public MembershipAlreadyExistsException(Guid workspaceId, string userId) 
        : base($"Membership already exists for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public MembershipAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipCreationFailedException : Exception
{
    public MembershipCreationFailedException() 
        : base("Membership creation failed due to system error.")
    {
    }

    public MembershipCreationFailedException(Guid workspaceId, string userId) 
        : base($"Failed to create membership for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public MembershipCreationFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipNotFoundException : Exception
{
    public MembershipNotFoundException() 
        : base("Membership not found.")
    {
    }

    public MembershipNotFoundException(Guid workspaceId, string userId) 
        : base($"Membership not found for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public MembershipNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipUpdateFailedException : Exception
{
    public MembershipUpdateFailedException() 
        : base("Membership update failed due to system error.")
    {
    }

    public MembershipUpdateFailedException(string message) 
        : base(message)
    {
    }

    public MembershipUpdateFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipDeletionFailedException : Exception
{
    public MembershipDeletionFailedException() 
        : base("Membership deletion failed due to system error.")
    {
    }

    public MembershipDeletionFailedException(string message) 
        : base(message)
    {
    }

    public MembershipDeletionFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}


public sealed class TransferOwnershipFirstException : Exception
{
    public TransferOwnershipFirstException() 
        : base("Transfer ownership first before leaving the workspace.")
    {
    }

    public TransferOwnershipFirstException(string message) 
        : base(message)
    {
    }

    public TransferOwnershipFirstException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class OnlyOwnerCanTransferOwnershipException : Exception
{
    public OnlyOwnerCanTransferOwnershipException() 
        : base("Only the owner can transfer ownership.")
    {
    }

    public OnlyOwnerCanTransferOwnershipException(string message) 
        : base(message)
    {
    }

    public OnlyOwnerCanTransferOwnershipException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class OnlyOwnerCanKickMemberException : Exception
{
    public OnlyOwnerCanKickMemberException() 
        : base("Only the owner can kick a member from the workspace.")
    {
    }

    public OnlyOwnerCanKickMemberException(string message) 
        : base(message)
    {
    }

    public OnlyOwnerCanKickMemberException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FailedToKickMemberException : Exception
{
    public FailedToKickMemberException() 
        : base("Failed to kick member from the workspace.")
    {
    }

    public FailedToKickMemberException(Guid workspaceId, string memberId) 
        : base($"Failed to kick member '{memberId}' from workspace '{workspaceId}'.")
    {
    }

    public FailedToKickMemberException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class AlreadyAdminException : Exception
{
    public AlreadyAdminException() 
        : base("User is already an admin in the workspace.")
    {
    }

    public AlreadyAdminException(Guid workspaceId, string userId) 
        : base($"User '{userId}' is already an admin in workspace '{workspaceId}'.")
    {
    }

    public AlreadyAdminException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipReadModelAlreadyExistsException : Exception
{
    public MembershipReadModelAlreadyExistsException() 
        : base("Membership read model already exists for this user in the workspace.")
    {
    }

    public MembershipReadModelAlreadyExistsException(Guid workspaceId, string userId) 
        : base($"Membership read model already exists for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public MembershipReadModelAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FailedToCreateMembershipReadModelException : Exception
{
    public FailedToCreateMembershipReadModelException() 
        : base("Failed to create membership read model due to system error.")
    {
    }

    public FailedToCreateMembershipReadModelException(Guid workspaceId, string userId) 
        : base($"Failed to create membership read model for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public FailedToCreateMembershipReadModelException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class MembershipReadModelNotFoundException : Exception
{
    public MembershipReadModelNotFoundException() 
        : base("Membership read model not found.")
    {
    }

    public MembershipReadModelNotFoundException(Guid workspaceId, string userId) 
        : base($"Membership read model not found for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public MembershipReadModelNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FailedToDeleteMembershipReadModelException : Exception
{
    public FailedToDeleteMembershipReadModelException() 
        : base("Failed to delete membership read model due to system error.")
    {
    }

    public FailedToDeleteMembershipReadModelException(Guid workspaceId, string userId) 
        : base($"Failed to delete membership read model for user '{userId}' in workspace '{workspaceId}'.")
    {
    }

    public FailedToDeleteMembershipReadModelException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class NotEnoughPermissionsException : Exception
{
    public NotEnoughPermissionsException() 
        : base("You do not have enough permissions to perform this action.")
    {
    }

    public NotEnoughPermissionsException(string message) 
        : base(message)
    {
    }

    public NotEnoughPermissionsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}