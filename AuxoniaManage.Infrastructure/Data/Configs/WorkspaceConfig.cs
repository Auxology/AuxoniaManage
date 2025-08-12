using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class WorkspaceConfig : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(w => w.OwnerId)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(1024);
        
        builder.Property(w => w.CreatedAt)
            .IsRequired();
        
        builder.Property(w => w.UpdatedAt)
            .IsRequired();
        
        builder.Property(w => w.LogoKey)
            .HasMaxLength(256);
        
        builder.HasIndex(w => new { w.OwnerId, w.Id })
            .IsUnique();
    }
}