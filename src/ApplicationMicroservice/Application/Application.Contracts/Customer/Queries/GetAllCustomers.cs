﻿using Application.Contracts.Validation;
using Application.Dto.Pages;

namespace Application.Contracts.Customer.Queries;

internal static class GetAllCustomers
{
    public record Query(int Page) : IValidatableRequest<Response>;

    public record Response(CustomerPageDto Page);
}