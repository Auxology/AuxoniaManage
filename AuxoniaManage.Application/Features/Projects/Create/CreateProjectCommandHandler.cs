using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Entities;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Project;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Projects.Create;

public sealed class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IStorageService _storageService;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateProjectCommandHandler
    (
        IProjectRepository projectRepository,
        IWorkspacePermissionService workspacePermissionService,
        IStorageService storageService,
        IPublishEndpoint publishEndpoint
    )
    {
        _projectRepository = projectRepository;
        _workspacePermissionService = workspacePermissionService;
        _storageService = storageService;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        Guard.Against.Null(request.WorkspaceId, nameof(request.WorkspaceId));
        Guard.Against.Null(request.UserId, nameof(request.UserId));
        
        var hasPermission = await _workspacePermissionService.IsOwnerAsync(
            request.WorkspaceId, 
            request.UserId, 
            cancellationToken);

        if (!hasPermission)
        {
            throw new UserDoesNotHavePermissionException();
        }

        var logoKey = request.Logo != null
            ? await _storageService.PutObjectAsync
            (
                file: request.Logo.OpenReadStream(),
                sender: Senders.Project,
                fileName: request.Logo.FileName,
                contentType: request.Logo.ContentType,
                fileSize: request.Logo.Length,
                cancellationToken
            )
            : null;

        var project = new Project
        (
            workspaceId: request.WorkspaceId,
            name: request.Name,
            timeStamp: DateTime.UtcNow,
            logoKey: logoKey
        );
        
        var isSuccess = await _projectRepository.AddAsync(project, cancellationToken);
        
        if (!isSuccess)
        {
            throw new FailedToCreateProjectException();
        }
        
        var projectCreatedEvent = new ProjectCreatedEvent
        (
            Id: project.Id,
            WorkspaceId: project.WorkspaceId,
            Name: project.Name,
            LogoKey: project.LogoKey
        );
        
        await _publishEndpoint.Publish(projectCreatedEvent, cancellationToken);
        
        return new CreateProjectResponse
        (
            Id: project.Id,
            WorkspaceId: project.WorkspaceId,
            Name: project.Name,
            LogoKey: project.LogoKey,
            CreatedAt: project.CreatedAt
        );
    }
}