using AuxoniaManage.Application.Pipelines;
using AuxoniaManage.Application.Services;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuxoniaManage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionalPipelineBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipelineBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        });
        
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumers(typeof(DependencyInjection).Assembly);

            cfg.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                config.ConfigureEndpoints(context);
            });
        });
        
        services.AddScoped<IWorkspacePermissionService, WorkspacePermissionService>();
        services.AddScoped<ICleanUpService, CleanUpService>();
        
        return services;
    }
}