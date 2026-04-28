using BurgerStack.Application.Services;
using BurgerStack.Domain.Dto;
using BurgerStack.Domain.Entity;
using BurgerStack.Domain.Interfaces.Repository;
using BurgerStack.Infrastracture.Repository.RepositoryUoW;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace BurgerStack.Test.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IDbContextTransaction> _transactionMock;

        public OrderServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _transactionMock = new Mock<IDbContextTransaction>();

            _repositoryUoWMock
                .Setup(x => x.OrderRepository)
                .Returns(_orderRepositoryMock.Object);

            _repositoryUoWMock
                .Setup(x => x.BeginTransaction())
                .Returns(_transactionMock.Object);
        }

        [Fact]
        public async Task Should_Add_Order_Successfully_When_Request_Is_Valid()
        {
            var request = new OrderCreateRequest
            {
                HasSandwich = true,
                HasFries = true,
                HasSoftDrink = true
            };

            _orderRepositoryMock
                .Setup(x => x.Add(It.IsAny<OrderEntity>()))
                .ReturnsAsync((OrderEntity orderEntity) => orderEntity);

            _repositoryUoWMock
                .Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask);

            _transactionMock
                .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _transactionMock
                .Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new OrderService(_repositoryUoWMock.Object);

            var result = await service.Add(request);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            Assert.True(result.Data.HasSandwich);
            Assert.True(result.Data.HasFries);
            Assert.True(result.Data.HasSoftDrink);

            Assert.Equal(9.50m, result.Data.Subtotal);
            Assert.Equal(0.20m, result.Data.DiscountPercentage);
            Assert.Equal(1.90m, result.Data.DiscountValue);
            Assert.Equal(7.60m, result.Data.Total);

            _orderRepositoryMock.Verify(x => x.Add(It.Is<OrderEntity>(o =>
                o.HasSandwich == request.HasSandwich &&
                o.HasFries == request.HasFries &&
                o.HasSoftDrink == request.HasSoftDrink &&
                o.Subtotal == 9.50m &&
                o.DiscountPercentage == 0.20m &&
                o.DiscountValue == 1.90m &&
                o.Total == 7.60m)), Times.Once);

            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
            _transactionMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transactionMock.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Return_Error_When_Order_Request_Has_No_Items()
        {
            var request = new OrderCreateRequest
            {
                HasSandwich = false,
                HasFries = false,
                HasSoftDrink = false
            };

            var service = new OrderService(_repositoryUoWMock.Object);

            var result = await service.Add(request);

            Assert.False(result.Success);
            Assert.Equal("O pedido deve conter pelo menos um item.", result.Message);

            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<OrderEntity>()), Times.Never);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Never);
            _transactionMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _transactionMock.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}