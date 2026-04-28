using BurgerStack.Domain.Entity;

namespace BurgerStack.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<OrderEntity> Add(OrderEntity orderEntity);
        OrderEntity Update(OrderEntity orderEntity);
        OrderEntity Delete(OrderEntity orderEntity);
        Task<List<OrderEntity>> Get();
        Task<OrderEntity?> GetById(int? id);
    }
}