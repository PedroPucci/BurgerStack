using BurgerStack.Application.ExtensionsErrors;
using BurgerStack.Domain.Entity;

namespace BurgerStack.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderEntity>> Add(OrderEntity orderEntity);
        Task Delete(int id);
        Task<List<OrderEntity>> Get();
        Task<Result<OrderEntity>> GetById(int id);
        Task<Result<OrderEntity>> Update(int id, OrderEntity orderEntity);
    }
}