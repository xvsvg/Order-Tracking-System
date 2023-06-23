﻿using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Queries;

internal static class GetCustomer
{
    public record Query(Guid Id) : IRequest<Result<Response>>;
    public record Response(CustomerDto? Customer);
}