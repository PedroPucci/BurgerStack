using BurgerStack.Domain.Dto;
using BurgerStack.Domain.Entity;
using BurgerStack.Domain.Interfaces.Repository;
using BurgerStack.Infrastracture.Connections;

namespace BurgerStack.Infrastracture.Repository.Request
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public Task<OrderCreateRequest> Add(OrderCreateRequest orderCreateRequest)
        {
            throw new NotImplementedException();
        }

        public OrderEntity Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<OrderEntity?> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public OrderEntity Update(OrderUpdateRequest OrderUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}