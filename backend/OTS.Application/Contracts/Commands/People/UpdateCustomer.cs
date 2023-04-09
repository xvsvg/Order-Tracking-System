using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.People;

internal static class UpdateCustomer
{
    public record Command(Guid Id, Guid OrderId) : IRequest<Response>;

    public record Response(CustomerDto Customer);
}