using BurgerStack.Domain.Dto;
using BurgerStack.Domain.Entity;

namespace BurgerStack.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<OrderCreateRequest> Add(OrderCreateRequest orderCreateRequest);
        OrderEntity Update(OrderUpdateRequest OrderUpdateRequest);
        OrderEntity Delete(int id);
        Task<List<OrderEntity>> Get();
        Task<OrderEntity?> GetById(int? id);
    }
}