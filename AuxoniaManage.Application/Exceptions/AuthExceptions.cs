namespace AuxoniaManage.Application.Exceptions;


public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() 
        : base("A user with this email address already exists.")
    {
    }
}

public sealed class UserRegistrationFailedException : Exception
{
    public UserRegistrationFailedException() 
        : base("User registration failed due to invalid data or system error.")
    {
    }

    public UserRegistrationFailedException(string message) 
        : base(message)
    {
    }
}

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() 
        : base("Invalid email or password.")
    {
    }
}

public sealed class UserLockedOutException : Exception
{
    public UserLockedOutException() 
        : base("User account is temporarily locked due to multiple failed login attempts.")
    {
    }
}


public sealed class EmailNotConfirmedException : Exception
{
    public EmailNotConfirmedException() 
        : base("Email address must be confirmed before login.")
    {
    }
}

public sealed class EmailVerificationFailedException : Exception
{
    public EmailVerificationFailedException() 
        : base("Email verification failed. The token may be invalid or expired.")
    {
    }
}

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException() 
        : base("User not found.")
    {
    }
}

public sealed class PasswordResetFailedException : Exception
{
    public PasswordResetFailedException() 
        : base("Password reset failed. The token may be invalid or expired.")
    {
    }
}

public sealed class NewEmailCannotBeSameAsOldEmailException : Exception
{
    public NewEmailCannotBeSameAsOldEmailException() 
        : base("The new email address cannot be the same as the old email address.")
    {
    }
}

public sealed class ChangeEmailRequestFailedException : Exception
{
    public ChangeEmailRequestFailedException() 
        : base("Change email request failed. The token may be invalid or expired.")
    {
    }
}

public sealed class OldPasswordCannotBeSameAsNewPasswordException : Exception
{
    public OldPasswordCannotBeSameAsNewPasswordException() 
        : base("The old password cannot be the same as the new password.")
    {
    }
}

public sealed class FailedToChangePasswordException : Exception
{
    public FailedToChangePasswordException() 
        : base("Failed to change password. please check the credentials and try again.")
    {
    }

    public FailedToChangePasswordException(string message) 
        : base(message)
    {
    }
}