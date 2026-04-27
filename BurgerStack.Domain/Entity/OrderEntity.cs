using BurgerStack.Domain.General;

namespace BurgerStack.Domain.Entity
{
    public class OrderEntity : BaseEntity
    {
        public bool HasSandwich { get; set; }
        public bool HasFries { get; set; }
        public bool HasSoftDrink { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Total { get; set; }
    }
}