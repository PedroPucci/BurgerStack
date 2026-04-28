using BurgerStack.Application.ExtensionsErrors;
using BurgerStack.Application.Services.Interfaces;
using BurgerStack.Domain.Dto;
using BurgerStack.Domain.Entity;
using BurgerStack.Infrastracture.Repository.RepositoryUoW;

namespace BurgerStack.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public OrderService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public Task<Result<OrderEntity>> Add(OrderCreateRequest orderCreateRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderEntity>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderEntity>> Update(int id, OrderUpdateRequest orderUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}