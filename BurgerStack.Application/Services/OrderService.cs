using BurgerStack.Application.ExtensionsErrors;
using BurgerStack.Application.Services.Interfaces;
using BurgerStack.Domain.Dto;
using BurgerStack.Domain.Entity;
using BurgerStack.Infrastracture.Repository.RepositoryUoW;
using BurgerStack.Shared.Logging;
using BurgerStack.Shared.Validator;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace BurgerStack.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public OrderService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<OrderEntity>> Add(OrderCreateRequest orderCreateRequest)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                if (orderCreateRequest == null)
                    return Result<OrderEntity>.Error("O pedido não pode ser nulo.");

                if (!orderCreateRequest.HasSandwich &&
                    !orderCreateRequest.HasFries &&
                    !orderCreateRequest.HasSoftDrink)
                {
                    return Result<OrderEntity>.Error("O pedido deve conter pelo menos um item.");
                }

                decimal subtotal = 0;

                if (orderCreateRequest.HasSandwich)
                    subtotal += 5.00m;

                if (orderCreateRequest.HasFries)
                    subtotal += 2.00m;

                if (orderCreateRequest.HasSoftDrink)
                    subtotal += 2.50m;

                decimal discountPercentage = 0;

                if (orderCreateRequest.HasSandwich && orderCreateRequest.HasFries && orderCreateRequest.HasSoftDrink)
                    discountPercentage = 0.20m;
                else if (orderCreateRequest.HasSandwich && orderCreateRequest.HasSoftDrink)
                    discountPercentage = 0.15m;
                else if (orderCreateRequest.HasSandwich && orderCreateRequest.HasFries)
                    discountPercentage = 0.10m;

                decimal discountValue = subtotal * discountPercentage;
                decimal total = subtotal - discountValue;

                var orderEntity = new OrderEntity
                {
                    HasSandwich = orderCreateRequest.HasSandwich,
                    HasFries = orderCreateRequest.HasFries,
                    HasSoftDrink = orderCreateRequest.HasSoftDrink,
                    Subtotal = subtotal,
                    DiscountPercentage = discountPercentage,
                    DiscountValue = discountValue,
                    Total = total
                };

                var isValid = await IsValidOrderRequest(orderEntity);
                if (!isValid.Success)
                {
                    Log.Information(isValid.Message);
                    return Result<OrderEntity>.Error(isValid.Message);
                }

                await _repositoryUoW.OrderRepository.Add(orderEntity);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(LogMessages.AddingOrderSuccess());
                return Result<OrderEntity>.Ok(orderEntity);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error(LogMessages.AddingOrderError(ex));
                return Result<OrderEntity>.Error($"Error to add a new Order: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetById(id);

                if (order == null)
                    return Result<bool>.Error("Pedido não encontrado.");

                _repositoryUoW.OrderRepository.Delete(order);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(LogMessages.DeleteOrderSuccess());
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error(LogMessages.DeleteOrderError(ex));
                return Result<bool>.Error($"Erro ao deletar pedido: {ex.Message}");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<List<OrderEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<OrderEntity> orders = await _repositoryUoW.OrderRepository.Get();
                _repositoryUoW.Commit();
                return orders;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllOrderError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao carregar a lista de pedidos");
            }
            finally
            {
                Log.Information(LogMessages.GetAllOrderSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<OrderEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetById(id);

                if (order == null)
                {
                    Log.Error($"Pedido com Id {id} não encontrado.");
                    return Result<OrderEntity>.Error("Pedido não encontrado.");
                }

                _repositoryUoW.Commit();
                return Result<OrderEntity>.Ok(order);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao buscar pedido por Id: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar pedido por Id.", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<OrderEntity>> Update(int id, OrderUpdateRequest orderUpdateRequest)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetById(id);

                if (order is null)
                    throw new InvalidOperationException("Erro ao atualizar Pedido");

                order.HasSandwich = orderUpdateRequest.HasSandwich;
                order.HasFries = orderUpdateRequest.HasFries;
                order.HasSoftDrink = orderUpdateRequest.HasSoftDrink;
                order.ModificationDate = DateTime.UtcNow;

                decimal subtotal = 0;

                if (order.HasSandwich)
                    subtotal += 5.00m;

                if (order.HasFries)
                    subtotal += 2.00m;

                if (order.HasSoftDrink)
                    subtotal += 2.50m;

                decimal discountPercentage = 0;

                if (order.HasSandwich && order.HasFries && order.HasSoftDrink)
                    discountPercentage = 0.20m;
                else if (order.HasSandwich && order.HasSoftDrink)
                    discountPercentage = 0.15m;
                else if (order.HasSandwich && order.HasFries)
                    discountPercentage = 0.10m;

                var discountValue = subtotal * discountPercentage;
                var total = subtotal - discountValue;

                order.Subtotal = subtotal;
                order.DiscountPercentage = discountPercentage;
                order.DiscountValue = discountValue;
                order.Total = total;

                _repositoryUoW.OrderRepository.Update(order);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<OrderEntity>.Ok(order);
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingOrderError(ex));
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Erro ao atualizar Pedido", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<OrderEntity>> IsValidOrderRequest(OrderEntity orderEntity)
        {
            var requestValidator = await new OrderRequestValidator().ValidateAsync(orderEntity);

            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<OrderEntity>.Error(errorMessage);
            }

            return Result<OrderEntity>.Ok();
        }
    }
}