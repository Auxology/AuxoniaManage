using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.DeleteReadModel;

public sealed record DeleteProjectReadModelCommand
(
    Guid ProjectId
) : ICommand<DeleteProjectReadModelResponse>;

public sealed record DeleteProjectReadModelResponse
(
    bool IsDeleted,
    Guid ProjectId
);