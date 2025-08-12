namespace AuxoniaManage.Application.Exceptions;

public sealed class ProfileAlreadyExistsException : Exception
{
    public ProfileAlreadyExistsException() 
        : base("A profile already exists for this user.")
    {
    }
}

public sealed class ProfileCreationFailedException : Exception
{
    public ProfileCreationFailedException() 
        : base("Profile creation failed due to invalid data or system error.")
    {
    }

    public ProfileCreationFailedException(string message) 
        : base(message)
    {
    }
}

public sealed class ProfileNotFoundException : Exception
{
    public ProfileNotFoundException() 
        : base("Profile not found.")
    {
    }

    public ProfileNotFoundException(string message) 
        : base(message)
    {
    }
}

public sealed class ProfileUpdateFailedException : Exception
{
    public ProfileUpdateFailedException() 
        : base("Profile update failed due to invalid data or system error.")
    {
    }

    public ProfileUpdateFailedException(string message) 
        : base(message)
    {
    }
}

public sealed class ProfileReadModelAlreadyExistsException : Exception
{
    public ProfileReadModelAlreadyExistsException() 
        : base("Profile read model already exists.")
    {
    }
}

public sealed class ProfileReadModelCreationFailedException : Exception
{
    public ProfileReadModelCreationFailedException() 
        : base("Failed to create profile read model.")
    {
    }
}

public sealed class ProfileReadModelNotFoundException : Exception
{
    public ProfileReadModelNotFoundException() 
        : base("Profile read model not found.")
    {
    }

    public ProfileReadModelNotFoundException(string message) 
        : base(message)
    {
    }
}

public sealed class ProfileReadModelUpdateFailedException : Exception
{
    public ProfileReadModelUpdateFailedException() 
        : base("Profile read model update failed due to invalid data or system error.")
    {
    }

    public ProfileReadModelUpdateFailedException(string message) 
        : base(message)
    {
    }
}