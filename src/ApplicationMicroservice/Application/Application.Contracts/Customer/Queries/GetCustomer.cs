﻿using Application.Dto;
using MediatR;

namespace Application.Contracts.Customer.Queries;

internal static class GetCustomer
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(CustomerDto? Customer);
}