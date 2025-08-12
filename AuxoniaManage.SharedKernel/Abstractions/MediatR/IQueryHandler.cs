using MediatR;

namespace AuxoniaManage.SharedKernel.Abstractions.MediatR;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    
}