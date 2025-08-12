using AuxoniaManage.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class ProjectReadModelConfig : IEntityTypeConfiguration<ProjectReadModel> 
{
    public void Configure(EntityTypeBuilder<ProjectReadModel> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.WorkspaceId)
            .IsRequired();
        
        builder.Property(x => x.ProjectId)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.LogoKey)
            .IsRequired(false)
            .HasMaxLength(256);
        
        builder.HasIndex(x => new { x.Id, x.ProjectId, x.WorkspaceId })
            .IsUnique();
    }
}