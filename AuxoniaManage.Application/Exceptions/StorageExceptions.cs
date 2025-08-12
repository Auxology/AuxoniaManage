namespace AuxoniaManage.Application.Exceptions;


public sealed class InvalidFileException : Exception
{
    public InvalidFileException() 
        : base("Invalid file type or size.")
    {
    }

    public InvalidFileException(string message) 
        : base(message)
    {
    }

    public InvalidFileException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FileUploadFailedException : Exception
{
    public FileUploadFailedException() 
        : base("Failed to upload file to storage.")
    {
    }

    public FileUploadFailedException(string message) 
        : base(message)
    {
    }

    public FileUploadFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FileDeletionFailedException : Exception
{
    public FileDeletionFailedException() 
        : base("Failed to delete file from storage.")
    {
    }

    public FileDeletionFailedException(string message) 
        : base(message)
    {
    }

    public FileDeletionFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class StorageServiceUnavailableException : Exception
{
    public StorageServiceUnavailableException() 
        : base("Storage service is currently unavailable.")
    {
    }

    public StorageServiceUnavailableException(string message) 
        : base(message)
    {
    }

    public StorageServiceUnavailableException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class UnsupportedFileTypeException : Exception
{
    public UnsupportedFileTypeException() 
        : base("File type is not supported.")
    {
    }

    public UnsupportedFileTypeException(string fileExtension) 
        : base($"File type '{fileExtension}' is not supported.")
    {
    }

    public UnsupportedFileTypeException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public sealed class FileSizeExceededException : Exception
{
    public FileSizeExceededException() 
        : base("File size exceeds the maximum allowed limit.")
    {
    }

    public FileSizeExceededException(long fileSize, long maxSize) 
        : base($"File size ({fileSize} bytes) exceeds the maximum allowed limit ({maxSize} bytes).")
    {
    }

    public FileSizeExceededException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}