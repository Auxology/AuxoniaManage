namespace AuxoniaManage.Infrastructure.Email.Configs;

public sealed class EmailSettings
{
    public const string Section = nameof(EmailSettings);

    public string SenderEmail { get; set; } = null!;
    
    public string BaseUrl { get; set; } = null!;
}