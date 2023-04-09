using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.People;

internal static class CreateCustomer
{
    public record Command
        (string Fullname, string Email, string Password, params string[] ContactInfo) : IRequest<Response>;

    public record Response(CustomerDto Customer);
}