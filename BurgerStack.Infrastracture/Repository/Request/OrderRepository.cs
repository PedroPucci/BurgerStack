using BurgerStack.Domain.Entity;
using BurgerStack.Domain.Interfaces.Repository;
using BurgerStack.Infrastracture.Connections;
using Microsoft.EntityFrameworkCore;

namespace BurgerStack.Infrastracture.Repository.Request
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OrderEntity> Add(OrderEntity orderEntity)
        {
            var result = await _context.Order.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public OrderEntity Delete(OrderEntity orderEntity)
        {
            var response = _context.Order.Remove(orderEntity);
            return response.Entity;
        }

        public async Task<List<OrderEntity>> Get()
        {
            return await _context.Order.ToListAsync();
        }

        public async Task<OrderEntity?> GetById(int? id)
        {
            if (id == null)
                return null;

            return await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
        }

        public OrderEntity Update(OrderEntity orderEntity)
        {
            var response = _context.Order.Update(orderEntity);
            return response.Entity;
        }
    }
}