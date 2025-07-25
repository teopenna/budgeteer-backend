using System.Net;
using FluentValidation;
using FluentValidation.Results;

namespace Budgeteer.ApiService.Common.Validation.Requests;

internal sealed class RequestValidationApiFilter<TRequestToValidate> : IEndpointFilter where TRequestToValidate : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        TRequestToValidate? requestToValidate = context.Arguments.FirstOrDefault(argument => argument?.GetType() == typeof(TRequestToValidate)) as TRequestToValidate;
        IValidator<TRequestToValidate>? validator = context.HttpContext.RequestServices.GetService<IValidator<TRequestToValidate>>();

        if (validator is null)
        {
            return await next.Invoke(context);
        }

        ValidationResult? validationResult = await validator.ValidateAsync(requestToValidate!);
        if (validationResult.IsValid)
        {
            return await next.Invoke(context);
        }

        IDictionary<string, string[]>? errors = validationResult.ToDictionary();
        return Results.ValidationProblem(errors,
            statusCode: (int)HttpStatusCode.BadRequest);
    }
}
