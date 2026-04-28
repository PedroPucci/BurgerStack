using BurgerStack.Application.Services;
using BurgerStack.Application.Services.Interfaces;
using BurgerStack.Application.UnitOfWork;
using BurgerStack.Domain.Interfaces.Repository;
using BurgerStack.Extensions.SwaggerDocumentation;
using BurgerStack.Infrastracture.Connections;
using BurgerStack.Infrastracture.Repository.RepositoryUoW;
using BurgerStack.Infrastracture.Repository.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BurgerStack.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Teste",
                    Description = @"
                        A **API BurgerStack** é uma solução para gerenciamento de pedidos de uma lanchonete.

                        Criada para registrar pedidos, consultar o cardápio e calcular automaticamente subtotal, desconto e valor final conforme as regras de combos.

                        **Principais Benefícios:**
                        - Cadastro e gerenciamento completo de pedidos.
                        - Consulta de cardápio disponível.
                        - Cálculo automático de descontos por combinação de itens.
                        - Validações claras para pedidos inválidos e itens duplicados.

                        Com a **BurgerStack**, a lanchonete pode controlar seus pedidos de forma simples, organizada e eficiente!
                        "
                });


                opt.OperationFilter<CustomOperationDescriptions>();
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("WebApiDatabase"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                });
            });
            services.AddScoped<IRepositoryUoW, RepositoryUoW>();
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();


            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }
    }
}
