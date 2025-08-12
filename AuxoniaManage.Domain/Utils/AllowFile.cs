namespace AuxoniaManage.Domain.Utils;

public sealed class AllowFile
{
    private static readonly IReadOnlySet<string> _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png"
    };
    
    private const long _maxFileSize = 2 * 1024 * 1024;
    
    public bool isValidFile(string extension, long fileSize)
    {
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            return false;
        }

        if (fileSize <= 0 || fileSize > _maxFileSize)
        {
            return false;
        }

        return true;
    }
}