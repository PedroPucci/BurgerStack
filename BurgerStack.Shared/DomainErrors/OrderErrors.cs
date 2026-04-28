using System.ComponentModel;

namespace BurgerStack.Shared.DomainErrors
{
    public enum OrderErrors
    {
        [Description("O pedido não pode ser nulo!")]
        Order_Error_CanNotBeNull,

        [Description("O pedido deve conter pelo menos um item!")]
        Order_Error_MustHaveAtLeastOneItem,

        [Description("O pedido deve conter um sanduíche!")]
        Order_Error_MustHaveOneSandwich,

        [Description("O pedido pode conter apenas um sanduíche!")]
        Order_Error_CanContainOnlyOneSandwich,

        [Description("O pedido pode conter apenas uma batata frita!")]
        Order_Error_CanContainOnlyOneFries,

        [Description("O pedido pode conter apenas um refrigerante!")]
        Order_Error_CanContainOnlyOneSoftDrink,

        [Description("Item do pedido inválido!")]
        Order_Error_InvalidItem,

        [Description("Pedido não encontrado!")]
        Order_Error_NotFound,

        [Description("Id do pedido inválido!")]
        Order_Error_InvalidId,

        [Description("Cálculo do subtotal inválido!")]
        Order_Error_InvalidSubtotal,

        [Description("Cálculo do desconto inválido!")]
        Order_Error_InvalidDiscount,

        [Description("Cálculo do total inválido!")]
        Order_Error_InvalidTotal
    }
}