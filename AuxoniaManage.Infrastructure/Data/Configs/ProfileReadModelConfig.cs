using AuxoniaManage.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class ProfileReadModelConfig : IEntityTypeConfiguration<ProfileReadModel>
{
    public void Configure(EntityTypeBuilder<ProfileReadModel> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.ProfileId)
            .IsRequired();
        
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(512);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.AvatarKey)
            .IsRequired(false)
            .HasMaxLength(256);
        
        builder.HasIndex(x => x.UserId)
            .IsUnique();
        
        builder.HasIndex(x => x.ProfileId)
            .IsUnique();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}