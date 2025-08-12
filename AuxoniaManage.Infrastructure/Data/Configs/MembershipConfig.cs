using AuxoniaManage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuxoniaManage.Infrastructure.Data.Configs;

public sealed class MembershipConfig : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(m => m.UserId)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(m => m.WorkspaceId)
            .IsRequired();
        
        builder.Property(m => m.JoinedAt)
            .IsRequired();
        
        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.Property(m => m.Role)
            .IsRequired();

        builder.HasIndex(m => new { m.UserId, m.WorkspaceId })
            .IsUnique();
    }
}