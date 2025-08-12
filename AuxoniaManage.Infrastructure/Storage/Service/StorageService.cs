using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Utils;
using AuxoniaManage.Infrastructure.Storage.Configs;
using Microsoft.Extensions.Options;

namespace AuxoniaManage.Infrastructure.Storage.Service;

public class StorageService : IStorageService
{
    private readonly AllowFile _allowFile;
    private readonly Generators _generators;
    private readonly IAmazonS3 _amazonS3;
    private readonly StorageSettings _storageSettings;

    public StorageService
    (
        IAmazonS3 amazonS3,
        AllowFile allowFile,
        Generators generators,
        IOptions<StorageSettings> storageSettings
    )

    {
        _amazonS3 = amazonS3;
        _allowFile = allowFile;
        _generators = generators;
        _storageSettings = storageSettings.Value;
    }
    
    public async Task<string> PutObjectAsync(Stream file, Senders sender, string fileName, string contentType, long fileSize,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(fileName);

        var isValid = _allowFile.isValidFile(extension, fileSize);
        
        if (!isValid)
        {
            throw new InvalidFileException("Invalid file type or size.");
        }
        
        var bucketName = _storageSettings.BucketName;
        
        var path = sender switch
        {
            Senders.Profile => _storageSettings.AvatarPath,
            Senders.Workspace => _storageSettings.LogoPath,
            Senders.Project => _storageSettings.ProjectPath,
            _ => throw new ArgumentOutOfRangeException(nameof(sender), sender, null)
        };
        
        var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var randomString = _generators.RandomVeryLongString;
        
        var key = $"{path}/{timeStamp}_{randomString}{extension}";
        
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = file,
            ContentType = contentType
        };
        
        var result = await _amazonS3.PutObjectAsync(putRequest, cancellationToken);
        
        if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new FileUploadFailedException("Failed to upload file to S3.");
        }
        
        return key;
    }

    public async Task<bool> DeleteObjectAsync(string key, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _storageSettings.BucketName,
            Key = key
        };
        
        var result = await _amazonS3.DeleteObjectAsync(deleteRequest, cancellationToken);
        
        if (result.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
        {
            throw new FileDeletionFailedException("Failed to delete file from S3.");
        }
        
        return true;
    }

    public Task<string> ConstructUrlAsync(string key, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        var cdnUrl = _storageSettings.CdnUrl;
        
        var url = $"{cdnUrl}/{key}";

        return Task.FromResult(url);
    }
}