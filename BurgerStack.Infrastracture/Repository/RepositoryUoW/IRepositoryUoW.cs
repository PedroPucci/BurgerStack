using BurgerStack.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace BurgerStack.Infrastracture.Repository.RepositoryUoW
{
    public interface IRepositoryUoW
    {
        IOrderRepository OrderRepository { get; }

        Task SaveAsync();
        void Commit();
        IDbContextTransaction BeginTransaction();
    }
}