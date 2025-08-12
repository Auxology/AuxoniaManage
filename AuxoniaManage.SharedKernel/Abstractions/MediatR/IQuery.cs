using MediatR;

namespace AuxoniaManage.SharedKernel.Abstractions.MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}