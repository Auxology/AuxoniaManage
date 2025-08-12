namespace AuxoniaManage.Domain.Entities;

public sealed class UserProfile
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Email { get; private set; }
    
    public string? AvatarKey { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    private UserProfile()
    {
        // Required for EF Core
    }
    
    public UserProfile(string userId, string firstName, string lastName, string email, DateTime timeStamp, string? avatarKey = null)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = timeStamp;
        UpdatedAt = timeStamp;
        AvatarKey = avatarKey;
    }
    
    public void UpdateProfile(string firstName, string lastName, DateTime timeStamp, string? avatarKey = null)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = timeStamp;
        AvatarKey = avatarKey;
    }
}