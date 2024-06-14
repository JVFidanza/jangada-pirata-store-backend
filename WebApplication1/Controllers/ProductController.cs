using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Dtos;
using WebApplication1.Interfaces.Service;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public IActionResult GetAllProducts([FromQuery] int take, [FromQuery] int skip = 0)
    {
        var allProducts = _productService.GetAllProducts(take, skip);
        
        return Ok(allProducts);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
    {
        var createdProduct = await _productService.AddProduct(productDto);
        
        return CreatedAtAction(nameof(GetProductById),
            new { id = createdProduct.Entity.Id },
            createdProduct);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        try
        {
            await _productService.UpdateProduct(id, updateProductDto);

            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchProduct(int id,
        [FromBody] JsonPatchDocument<UpdateProductDto> patch)
    {
        try
        {
            await _productService.PatchProduct(id, patch);

            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }        
        catch (ValidationException e)
        {
            return ValidationProblem(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}