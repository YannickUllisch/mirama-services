

namespace Mirama.SharedKernel.Abstractions.Common.Interfaces;

public interface ICommand
{
}

public interface IQuery
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand
{
}

public interface IQuery<out TResponse> : IRequest<TResponse>, IQuery
{
}
