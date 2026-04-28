using BurgerStack.Application.Services;

namespace BurgerStack.Application.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        OrderService OrderService { get; }
    }
}