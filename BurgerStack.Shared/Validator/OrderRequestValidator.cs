using BurgerStack.Domain.Entity;
using BurgerStack.Shared.DomainErrors;
using BurgerStack.Shared.Helpers;
using FluentValidation;

namespace BurgerStack.Shared.Validator
{
    public class OrderRequestValidator : AbstractValidator<OrderEntity>
    {
        public OrderRequestValidator()
        {
            RuleFor(p => p)
                .NotNull()
                .WithMessage(OrderErrors.Order_Error_CanNotBeNull.Description());

            RuleFor(p => p)
                .Must(p => p.HasSandwich || p.HasFries || p.HasSoftDrink)
                .WithMessage(OrderErrors.Order_Error_MustHaveAtLeastOneItem.Description());

            RuleFor(p => p.HasSandwich)
                .Equal(true)
                .When(p => p.HasFries || p.HasSoftDrink)
                .WithMessage(OrderErrors.Order_Error_MustHaveOneSandwich.Description());

            RuleFor(p => p.Subtotal)
                .GreaterThanOrEqualTo(0)
                .WithMessage(OrderErrors.Order_Error_InvalidSubtotal.Description());

            RuleFor(p => p.DiscountPercentage)
                .InclusiveBetween(0, 0.20m)
                .WithMessage(OrderErrors.Order_Error_InvalidDiscount.Description());

            RuleFor(p => p.Total)
                .GreaterThanOrEqualTo(0)
                .WithMessage(OrderErrors.Order_Error_InvalidTotal.Description());
        }
    }
}