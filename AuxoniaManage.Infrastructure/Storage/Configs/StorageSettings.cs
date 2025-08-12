namespace AuxoniaManage.Infrastructure.Storage.Configs;

public sealed class StorageSettings
{
    public string RegionName { get; set; } = string.Empty;
    
    public string BucketName { get; set; } = string.Empty;
    
    public string AvatarPath { get; set; } = string.Empty;
    
    public string LogoPath { get; set; } = string.Empty;
    
    public string ProjectPath { get; set; } = string.Empty;
    
    public string CdnUrl { get; set; } = string.Empty;
}