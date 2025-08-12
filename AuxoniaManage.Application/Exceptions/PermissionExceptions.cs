namespace AuxoniaManage.Application.Exceptions;

public sealed class YouAreNotMemberOfWorkspaceException : Exception
{
    public YouAreNotMemberOfWorkspaceException()
        : base("You are not a member of this workspace.")
    {
    }

    public YouAreNotMemberOfWorkspaceException(string message)
        : base(message)
    {
    }

    public YouAreNotMemberOfWorkspaceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class InsufficientPermissionsException : Exception
{
    public InsufficientPermissionsException()
        : base("You do not have sufficient permissions to perform this action.")
    {
    }

    public InsufficientPermissionsException(string message)
        : base(message)
    {
    }

    public InsufficientPermissionsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class OneOrMoreAssigneesNotMemberOfWorkspaceException : Exception
{
    public OneOrMoreAssigneesNotMemberOfWorkspaceException()
        : base("One or more assignees are not members of this workspace.")
    {
    }

    public OneOrMoreAssigneesNotMemberOfWorkspaceException(string message)
        : base(message)
    {
    }

    public OneOrMoreAssigneesNotMemberOfWorkspaceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class CannotAssignRolesToHigherHierarchyException : Exception
{
    public CannotAssignRolesToHigherHierarchyException() 
        : base("You cannot assign roles to users with equal or higher privileges.")
    {
    }

    public CannotAssignRolesToHigherHierarchyException(string message) 
        : base(message)
    {
    }

    public CannotAssignRolesToHigherHierarchyException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}