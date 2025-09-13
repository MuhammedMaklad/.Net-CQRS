using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MinimalApi.Filters;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
  private readonly IValidator<T> _validator;
  private readonly ILogger<ValidationFilter<T>> _logger;

  public ValidationFilter(IValidator<T> validator, ILogger<ValidationFilter<T>> logger)
  {
    _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async ValueTask<object?> InvokeAsync(
      EndpointFilterInvocationContext context,
      EndpointFilterDelegate next)
  {
    var requestObject = ExtractRequestObject(context);
    if (requestObject is null)
    {
      return HandleMissingRequestObject();
    }

    var validationResult = await _validator.ValidateAsync(requestObject, context.HttpContext.RequestAborted);
    if (!validationResult.IsValid)
    {
      return HandleValidationFailure(validationResult);
    }

    return await next(context);
  }

  private T? ExtractRequestObject(EndpointFilterInvocationContext context)
  {
    return context.Arguments
        .OfType<T>()
        .FirstOrDefault();
  }

  private IResult HandleMissingRequestObject()
  {
    const string errorMessage = "Invalid request format - expected object not found";

    _logger.LogWarning(
        "No object of type {ObjectType} found in request context",
        typeof(T).Name);

    return Results.BadRequest(errorMessage);
  }

  private IResult HandleValidationFailure(FluentValidation.Results.ValidationResult validationResult)
  {
    var errors = validationResult.Errors
        .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
        .ToArray();

    var errorMessages = errors.Select(e => e.Message).ToArray();

    _logger.LogInformation(
        "Validation failed for {ObjectType}. Errors: {@ValidationErrors}",
        typeof(T).Name,
        errors);

    return Results.BadRequest(
        errorMessages);
  }
}

// Supporting types for better error handling
public record ValidationError(string PropertyName, string Message);

// Extension method for easier registration
public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidation<T>(this RouteHandlerBuilder builder) 
        where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}

// Usage example:
// app.MapPost("/api/users", CreateUser)
//    .AddValidation<CreateUserRequest>();