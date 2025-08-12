using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Application.Features.Workspace.CreateReadModel;
using AuxoniaManage.Application.Services;
using AuxoniaManage.Domain.Enums;
using AuxoniaManage.Domain.Utils;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;
using MediatR;

namespace AuxoniaManage.Application.Features.Workspace.Create;

public sealed class CreateWorkspaceCommandHandler : ICommandHandler<CreateWorkspaceCommand, CreateWorkspaceResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IStorageService _storageService;
    private readonly IMediator _mediator;
    private readonly Generators _generators;

    public CreateWorkspaceCommandHandler
    (
        IWorkspaceRepository workspaceRepository, 
        IStorageService storageService,
        IMediator mediator,
        Generators generators
    )

    {
        _workspaceRepository = workspaceRepository;
        _storageService = storageService;
        _mediator = mediator;
        _generators = generators;
    }

    public async Task<CreateWorkspaceResponse> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        Guard.Against.NullOrEmpty(request.OwnerId, nameof(request.OwnerId));
        Guard.Against.NullOrEmpty(request.Description, nameof(request.Description));
        
        string? logoKey = null;
        var invitationToken = _generators.RandomVeryLongString;

        if (request.Logo != null)
        {
            logoKey = await _storageService.PutObjectAsync(
                file: request.Logo.OpenReadStream(),
                sender: Senders.Workspace,
                fileName: request.Logo.FileName,
                contentType: request.Logo.ContentType,
                fileSize: request.Logo.Length,
                cancellationToken: cancellationToken
            );
        }

        var workspace = new Domain.Entities.Workspace
        (
            name: request.Name,
            description: request.Description,
            ownerId: request.OwnerId,
            invitationToken: invitationToken,
            timeStamp: DateTime.UtcNow,
            logoKey: logoKey
        );
        
        var isSuccess = await _workspaceRepository.AddAsync(workspace, cancellationToken);
        
        if (!isSuccess)
        {
            throw new WorkspaceCreationFailedException("Failed to create workspace");
        }
        
        var createWorkspaceReadModelCommand = new CreateWorkspaceReadModelCommand
        (
            WorkspaceId: workspace.Id,
            Name: workspace.Name,
            LogoKey: workspace.LogoKey
        );
        
        await _mediator.Send(createWorkspaceReadModelCommand, cancellationToken);
        
        return new CreateWorkspaceResponse
        (
            Id: workspace.Id,
            OwnerId: workspace.OwnerId,
            Name: workspace.Name,
            Description: workspace.Description,
            CreatedAt: workspace.CreatedAt,
            LogoKey: workspace.LogoKey
        );
    }
}