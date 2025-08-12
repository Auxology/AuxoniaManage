namespace AuxoniaManage.Domain.ReadModels;

public sealed class ProfileReadModel
{
    public Guid Id { get; private set; }
    
    public Guid ProfileId { get; private set; }
    
    public string UserId { get; private set; }
    
    public string FullName { get; private set; }
    
    public string Email { get; private set; }
    
    public string? AvatarKey { get; private set; }
    
    private ProfileReadModel() { }
    
    public ProfileReadModel(Guid profileId, string userId, string fullName, string email, string? avatarKey)
    {
        ProfileId = profileId;
        UserId = userId;
        FullName = fullName;
        Email = email;
        AvatarKey = avatarKey;
    }
    
    public void UpdateReadModel(string fullName, string? avatarKey)
    {
        FullName = fullName;
        AvatarKey = avatarKey;
    }
}