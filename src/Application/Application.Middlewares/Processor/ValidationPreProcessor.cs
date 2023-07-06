﻿using Application.Contracts.Validation;
using FluentValidation;
using MediatR.Pipeline;

namespace Application.Middlewares.Processor;

public class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : IValidatableRequest
{
    private readonly IValidator<TRequest> _validator;

    public ValidationPreProcessor(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
    }
}