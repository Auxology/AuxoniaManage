using AuxoniaManage.Domain.Entities;
using AuxoniaManage.Domain.ReadModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuxoniaManage.Infrastructure.Data;

public sealed class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserProfile> UserProfiles { get; set; }
    
    public DbSet<Workspace> Workspaces { get; set; }
    
    public DbSet<Membership> Memberships { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    
    public DbSet<ProfileReadModel> ProfileReadModels { get; set; }
    
    public DbSet<WorkspaceReadModel> WorkspaceReadModels { get; set; }
    
    
    public DbSet<ProjectReadModel> ProjectReadModels { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Additional model configurations can be added here
    }
}