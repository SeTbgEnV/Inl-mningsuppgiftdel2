using Microsoft.AspNetCore.Mvc;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet()]
    public async Task<ActionResult> ListAllOrders()
    {
        var orders = await _unitOfWork.OrderRepository.ListAllOrders();
        return Ok(new { success = true, data = orders });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindOrder(int id)
    {
        var order = await _unitOfWork.OrderRepository.FindOrder(id);
        return Ok(new { success = true, data = order });
    }
    [HttpGet("Date/{date}")]
    public async Task<ActionResult> FindByDate(string date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return BadRequest(new { success = false, message = "Datum saknas" });
        }
        else
        {
            var orders = await _unitOfWork.OrderRepository.FindByDate(date);
            return Ok(new { success = true, data = orders });
        }
    }

    [HttpPost()]
    public async Task<IActionResult> AddOrder(SalesOrderViewModel order)
    {
        try
        {
            var result = await _unitOfWork.OrderRepository.AddOrder(order);
            if (result != null)
            {
                if (await _unitOfWork.Complete())
                {
                    return StatusCode(201, new { success = true, data = result });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Ingen beställning blev skapad"});
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Gick inte att lägga beställning" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            if (await _unitOfWork.OrderRepository.DeleteOrder(id))
            {
                if (_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { success = false, message = "Inget har tagits bort" });
                }
            }
            else
            {
                return StatusCode(500);
            }
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }
}