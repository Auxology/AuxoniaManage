using AuxoniaManage.Domain.Entities;
using AuxoniaManage.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(x => x.WorkspaceId)
            .IsRequired();

        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        
        builder.Property(x => x.LogoKey)
            .IsRequired(false)
            .HasMaxLength(256);
        
        builder.HasIndex(x => new { x.Id, x.WorkspaceId })
            .IsUnique();
    }
}