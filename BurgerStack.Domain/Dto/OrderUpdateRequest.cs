namespace BurgerStack.Domain.Dto
{
    public class OrderUpdateRequest
    {
        public bool HasSandwich { get; set; }
        public bool HasFries { get; set; }
        public bool HasSoftDrink { get; set; }
    }
}