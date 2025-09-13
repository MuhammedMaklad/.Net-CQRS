using FluentValidation;
using OrderApi.Commands;

namespace OrderApi;

public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
{
  public CreateOrderCommandValidator()
  {
    RuleFor(model => model.FirstName).NotEmpty()
    .MaximumLength(100);

    RuleFor(model => model.LastName).NotEmpty()
    .MaximumLength(100);

    RuleFor(model => model.Status).NotEmpty();
    RuleFor(model => model.Status).NotEmpty();
    RuleFor(model => model.TotalCost).NotEmpty()
    .GreaterThan(0);

  }
}
