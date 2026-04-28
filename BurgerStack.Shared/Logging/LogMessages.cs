namespace BurgerStack.Shared.Logging
{
    public static class LogMessages
    {
        // Order
        public static string InvalidOrderInputs() => "Message: Entradas inválidas para Pedido.";

        public static string AddingOrderSuccess() => "Message: Pedido criado com sucesso.";
        public static string AddingOrderError(Exception ex) => $"Message: Erro ao criar um novo Pedido: {ex.Message}";

        public static string DeleteOrderSuccess() => "Message: Pedido removido com sucesso.";
        public static string DeleteOrderError(Exception ex) => $"Message: Erro ao remover Pedido: {ex.Message}";

        public static string GetAllOrderSuccess() => "Message: Pedidos carregados com sucesso.";
        public static string GetAllOrderError(Exception ex) => $"Message: Erro ao carregar a lista de Pedidos: {ex.Message}";

        public static string GetOrderByIdSuccess() => "Message: Pedido encontrado com sucesso.";
        public static string GetOrderByIdError(Exception ex) => $"Message: Erro ao consultar Pedido por Id: {ex.Message}";

        public static string UpdatingOrderSuccess() => "Message: Pedido atualizado com sucesso.";
        public static string UpdatingOrderError(Exception ex) => $"Message: Erro ao atualizar Pedido: {ex.Message}";

        public static string OrderNotFound() => "Message: Pedido não encontrado.";
        public static string OrderInvalidId() => "Message: Id do Pedido inválido.";

        public static string OrderMustHaveAtLeastOneItem() => "Message: O Pedido deve conter pelo menos um item.";
        public static string OrderCanContainOnlyOneSandwich() => "Message: O Pedido pode conter apenas um sanduíche.";
        public static string OrderCanContainOnlyOneFries() => "Message: O Pedido pode conter apenas uma batata frita.";
        public static string OrderCanContainOnlyOneSoftDrink() => "Message: O Pedido pode conter apenas um refrigerante.";

        public static string InvalidOrderSubtotal() => "Message: Cálculo do subtotal do Pedido inválido.";
        public static string InvalidOrderDiscount() => "Message: Cálculo do desconto do Pedido inválido.";
        public static string InvalidOrderTotal() => "Message: Cálculo do total do Pedido inválido.";
    }
}