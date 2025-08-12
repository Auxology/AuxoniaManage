using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.Id);
        
        builder.Property(up => up.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(up => up.UserId)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(up => up.FirstName)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(up => up.LastName)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(up => up.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(up => up.CreatedAt)
            .IsRequired();
        
        builder.Property(up => up.UpdatedAt)
            .IsRequired();
        
        builder.HasIndex(up => up.UserId)
            .IsUnique();
    }
}