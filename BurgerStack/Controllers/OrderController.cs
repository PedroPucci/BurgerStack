using BurgerStack.Application.UnitOfWork;
using BurgerStack.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BurgerStack.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public OrderController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] OrderCreateRequest orderCreateRequest)
        {
            var result = await _uow.OrderService.Add(orderCreateRequest);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrderUpdateRequest orderUpdateRequest)
        {
            var result = await _uow.OrderService.Update(id, orderUpdateRequest);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _uow.OrderService.Delete(id);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _uow.OrderService.Get();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _uow.OrderService.GetById(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}