using BurgerStack.Application.Services;
using BurgerStack.Infrastracture.Repository.RepositoryUoW;

namespace BurgerStack.Application.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private OrderService orderService;

        public UnitOfWorkService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public OrderService OrderService
        {
            get
            {
                if (orderService is null)
                    orderService = new OrderService(_repositoryUoW);
                return orderService;
            }
        }
    }
}