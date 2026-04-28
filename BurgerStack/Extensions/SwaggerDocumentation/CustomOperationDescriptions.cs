using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BurgerStack.Extensions.SwaggerDocumentation
{
    public class CustomOperationDescriptions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.ApiDescription?.HttpMethod is null || context.ApiDescription.RelativePath is null)
                return;

            var path = context.ApiDescription.RelativePath.ToLowerInvariant();

            var routeHandlers = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                { "orders",               () => HandleOrderOperations(operation, context) }
            };

            foreach (var kv in routeHandlers
                     .OrderByDescending(k => k.Key.Length))
            {
                if (path.Contains(kv.Key))
                {
                    kv.Value.Invoke();
                    return;
                }
            }
        }

        private void HandleOrderOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Criar novo pedido.";
                operation.Description = "Esse endpoint é responsável por criar um novo pedido, aplicando as regras de cálculo de subtotal, desconto e valor total.";
                AddResponses(operation, "200", "Pedido criado com sucesso.");
                AddResponses(operation, "400", "Dados do pedido inválidos.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Atualizar pedido.";
                operation.Description = "Esse endpoint é responsável por atualizar um pedido existente com base no ID informado.";
                AddResponses(operation, "200", "Pedido atualizado com sucesso.");
                AddResponses(operation, "404", "Pedido não encontrado.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Deletar pedido.";
                operation.Description = "Esse endpoint é responsável por remover um pedido com base no ID informado.";
                AddResponses(operation, "200", "Pedido deletado com sucesso.");
                AddResponses(operation, "404", "Pedido não encontrado.");
            }
            else if (method == "GET")
            {
                if (path.Contains("all"))
                {
                    operation.Summary = "Listar todos os pedidos.";
                    operation.Description = "Esse endpoint retorna todos os pedidos cadastrados.";
                    AddResponses(operation, "200", "Pedidos retornados com sucesso.");
                }
                else
                {
                    operation.Summary = "Buscar pedido por ID.";
                    operation.Description = "Esse endpoint retorna um pedido específico com base no ID informado.";
                    AddResponses(operation, "200", "Pedido encontrado com sucesso.");
                    AddResponses(operation, "404", "Pedido não encontrado.");
                }
            }
        }

        private void AddResponses(OpenApiOperation operation, string statusCode, string description)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse { Description = description });
            }
        }
    }
}