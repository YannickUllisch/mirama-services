
namespace Mirama.SharedKernel.Abstractions.Common.Interfaces;

/// <summary>
/// Marker interface for a request with a response.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the request.</typeparam>
public interface IRequest<out TResponse>
{
}
