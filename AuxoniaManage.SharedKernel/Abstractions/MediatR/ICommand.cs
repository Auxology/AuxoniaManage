using MediatR;

namespace AuxoniaManage.SharedKernel.Abstractions.MediatR;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}

public interface ITransactionalCommand : ICommand
{
    
}

public interface ITransactionalCommand<out TResponse> : ICommand<TResponse>
{
    
}