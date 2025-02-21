using Microsoft.AspNetCore.Mvc;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Product;

namespace MormorDagnysDel2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  [HttpGet()]
  public async Task<ActionResult> ListAllProducts()
  {
    return Ok(new { success = true, data = await _unitOfWork.ProductRepository.List() });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> FindProduct(int id)
  {
    var product = await _unitOfWork.ProductRepository.Get(id);
    if (product != null)
    {
      return Ok(new { success = true, data = product });
    }
    else
    {
      return NotFound(new { success = false, message = $"Tyvärr kunde vi inte hitta någon produkt med id {id}" });
    }
  }
  [HttpGet("{id}/orders")]
  public async Task<ActionResult> OrdersByProduct(int id)
  {
    var prduct = await _unitOfWork.ProductRepository.Get(id);
    if (prduct == null)
    {
      return NotFound(new { success = false, message = $"Produkten med id {id} existerar inte" });
    }
    var orders = await _unitOfWork.ProductRepository.OrdersByProduct(id);
    if (orders == null)
    {
      return NotFound(new { success = false, message = $"Inga beställningar hittades för produkten med id {id}" });
    }
    return Ok(new { success = true, data = orders });
  }

  [HttpPost()]
  public async Task<ActionResult> Add(ProductPostViewModel model)
  {
    if (model == null)
    {
      return BadRequest(new { success = false, message = $"Produkten med artikelnummer {model.ItemNumber} existerar redan" });
    }
    try
    {
      var newprod = await _unitOfWork.ProductRepository.Add(model);
      await _unitOfWork.Complete();
      return CreatedAtAction(nameof(FindProduct), new { id = newprod.Id }, newprod);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> UpdateProductPrice(int id, ProductPriceViewModel model)
  {
    var prod = await _unitOfWork.ProductRepository.UpdateProductPrice(id, model);

    if (prod == null)
    {
      return NotFound(new { success = false, message = $"Produkten som du försöker uppdatera existerar inte längre {0}", id });
    }
    try
    {
      prod.Price = model.Price;
      await _unitOfWork.Complete();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }

    return NoContent();
  }
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    try
    {
      var result = await _unitOfWork.ProductRepository.Delete(id);
      if (result)
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
    }
    catch (Exception e)
    {
      return BadRequest(new { success = false, message = e.Message });
    }
    return StatusCode(500);
  }
}