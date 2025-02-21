using Microsoft.AspNetCore.Mvc;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Customer;

namespace MormorDagnysDel2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet()]
    public async Task<ActionResult> GetAllCustomers()
    {
        var customers = await _unitOfWork.CustomerRepository.List();
        return Ok(new { success = true, data = customers });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCustomer(int id)
    {
        try
        {
            var customer = await _unitOfWork.CustomerRepository.Find(id);
            if (customer == null)
            {
                return NotFound(new { success = false, message = "Kunden hittades inte" });
            }
            return Ok(new { success = true, data = customer });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddCustomer(CustomerPostViewModel model)
    {
        try
        {
            var result = await _unitOfWork.CustomerRepository.Add(model);
            if (result)
            {
                if (await _unitOfWork.Complete())
                {
                    return StatusCode(201);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPut()]
    public async Task<ActionResult> UpdateCustomer(int id, CustomerBaseViewModel model)
    {
        try
        {
            var result = await _unitOfWork.CustomerRepository.Update(id, model);
            if (result == true)
            {
                if (await _unitOfWork.Complete())
                {
                    return StatusCode(204);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}