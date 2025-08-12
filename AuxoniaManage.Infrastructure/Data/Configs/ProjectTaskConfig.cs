using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class ProjectTaskConfig : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(x => x.ProjectId)
            .IsRequired();
        
        builder.Property(x => x.AssigneeIds)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(x => x.AssignedById)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        
        builder.Property(x => x.DeadlineAt)
            .IsRequired(false);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Status)
            .IsRequired();
        
        builder.Property(x => x.Priority)
            .IsRequired();
        
        builder.HasIndex(x => new { x.ProjectId, x.Id })
            .IsUnique();
    }
}