using AuxoniaManage.SharedKernel.Abstractions.MediatR;

namespace AuxoniaManage.Application.Features.Projects.DeleteManyReadModels;

public sealed record DeleteProjectsReadModelCommand
(
    IReadOnlyCollection<Guid> ProjectIds
) : ICommand<DeleteProjectsReadModelResponse>;

public sealed record DeleteProjectsReadModelResponse
(
    IReadOnlyCollection<Guid> Ids,
    IReadOnlyCollection<Guid> ProjectIds,
    DateTime DeletedAt
);