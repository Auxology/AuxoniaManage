using Ardalis.GuardClauses;
using AuxoniaManage.Application.Exceptions;
using AuxoniaManage.Domain.Utils;
using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Workspace.RotateInvitation;

public sealed class RotateInvitationCommandHandler : ICommandHandler<RotateInvitationCommand, RotateInvitationResponse>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly Generators _generators;

    public RotateInvitationCommandHandler
    (
        IWorkspaceRepository workspaceRepository,
        Generators generators
    )
    {
        _workspaceRepository = workspaceRepository;
        _generators = generators;
    }


    public async Task<RotateInvitationResponse> Handle(RotateInvitationCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Default(request.WorkspaceId, nameof(request.WorkspaceId));
        
        var workspace = await _workspaceRepository.GetAsync(request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            throw new WorkspaceNotFoundException();
        }
        
        if (workspace.OwnerId != request.UserId)
        {
            throw new OnlyOwnerCanRotateInvitationException();
        }
        
        var newInvitationCode = _generators.RandomVeryLongString;
        
        var timeStamp = DateTime.UtcNow;
        
        workspace.UpdateInvitationToken(newInvitationCode, timeStamp);
        
        return new RotateInvitationResponse
        (
            request.WorkspaceId,
            request.UserId,
            newInvitationCode,
            timeStamp
        );
    }
}