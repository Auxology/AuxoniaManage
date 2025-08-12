using AuxoniaManage.Domain.Enums;

namespace AuxoniaManage.Application.Services;

public interface IStorageService
{
    Task<string> PutObjectAsync(Stream file, Senders sender, string fileName, string contentType, long fileSize, CancellationToken cancellationToken);
    
    Task<bool> DeleteObjectAsync(string key, CancellationToken cancellationToken);
    
    Task<string> ConstructUrlAsync(string key, CancellationToken cancellationToken);
}