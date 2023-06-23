using MediatR;

namespace Application.Contracts.Validation;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequest
{
}

public interface IValidatableRequest
{
}