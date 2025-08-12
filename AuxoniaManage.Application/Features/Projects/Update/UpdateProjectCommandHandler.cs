using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Events.Storage;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MassTransit;

namespace AuxoniaManage.Application.Features.Projects.Update;

public sealed class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStorageService _storageService;
    private readonly IWorkspacePermissionService _workspacePermissionService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateProjectCommandHandler
    (
        IProjectRepository projectRepository,
        IStorageService storageService,
        IWorkspacePermissionService workspacePermissionService,
        IPublishEndpoint publishEndpoint
    )

    {
        _projectRepository = projectRepository;
        _storageService = storageService;
        _workspacePermissionService = workspacePermissionService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UpdateProjectResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.Id, nameof(request.Id));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var hasPermission = await _workspacePermissionService.IsOwnerAsync(
            request.WorkspaceId, request.UserId, cancellationToken);

        if (!hasPermission)
        {
            throw new UserDoesNotHavePermissionException();
        }
        
        var project = await _projectRepository.GetAsync(request.Id, cancellationToken);
        
        if (project == null || project.WorkspaceId != request.WorkspaceId)
        {
            throw new ProjectNotFoundException();
        }
        
        var projectName = request.Name ?? project.Name;
        var oldKey = project.LogoKey;
        var newKey = oldKey;

        if (request.Logo != null)
        {
            if (!string.IsNullOrEmpty(oldKey))
            {
                var objectRemovedEvent = new ObjectRemovedEvent
                (
                    oldKey
                );
                
                await _publishEndpoint.Publish(objectRemovedEvent, cancellationToken);
            }
            
            newKey = await _storageService.PutObjectAsync
                (
                    file: request.Logo.OpenReadStream(),
                    sender: Senders.Project,
                    fileName: request.Logo.FileName,
                    contentType: request.Logo.ContentType,
                    fileSize: request.Logo.Length,
                    cancellationToken: cancellationToken
                );
        }
        
        var timeStamp = DateTime.UtcNow;
        
        project.UpdateProject
        (
            name: projectName,
            logoKey: newKey,
            timeStamp: timeStamp
        );
        
        var isSuccess = await _projectRepository.UpdateAsync(project, cancellationToken);

        if (!isSuccess)
        {
            throw new FailedToUpdateProjectException();
        }
        
        return new UpdateProjectResponse
        (
            Id: project.Id,
            Name: project.Name,
            LogoUrl: !string.IsNullOrEmpty(project.LogoKey)
                ? await _storageService.ConstructUrlAsync(project.LogoKey, cancellationToken)
                : null,
            UpdatedAt: timeStamp,
            UpdatedById: request.UserId
        );
    }
}