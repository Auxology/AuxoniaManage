using Amazon;
using Amazon.S3;
using Amazon.SimpleEmail;
using AuxoniaManage.Application.Features.Membership;
using AuxoniaManage.Application.Features.Profile;
using AuxoniaManage.Application.Features.Projects;
using AuxoniaManage.Application.Features.ProjectTask;
using AuxoniaManage.Application.Features.Workspace;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Infrastructure.Data;
using AuxoniaManage.Infrastructure.Data.Repositories;
using AuxoniaManage.Infrastructure.Email.Configs;
using AuxoniaManage.Infrastructure.Email.Services;
using AuxoniaManage.Infrastructure.Storage.Configs;
using AuxoniaManage.Infrastructure.Storage.Service;
using AuxoniaManage.SharedKernel.Abstractions.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace AuxoniaManage.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                options.SignIn.RequireConfirmedAccount = true;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
    
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
    
            options.Events.OnRedirectToLogout = context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            };
        });
        
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IReadModelRepository, ReadModelRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
        
        services.Configure<StorageSettings>(configuration.GetSection("StorageSettings"));

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<StorageSettings>>().Value;

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(settings.RegionName)
            };

            return new AmazonS3Client(config);
        });

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IStorageService, StorageService>();

        services.AddOpenTelemetry()
            .WithLogging(logging =>
            {
                logging.AddOtlpExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddAWSInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsqlInstrumentation()
                    .AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddAWSInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter();
            });

        return services;
    }
}