using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private StoreContext _context;
    private IMapper _mapper;

    public ProductController(StoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult GetAllProducts([FromQuery] int take, [FromQuery] int skip = 0)
    {
        var productList = _context.Products;
        
        return take != default ? Ok(productList.Skip(skip).Take(take)) : Ok(productList.Skip(skip));
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
    {
        Product product = _mapper.Map<Product>(productDto); 
        
        var createdProduct = await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProductById),
            new { id = createdProduct.Entity.Id },
            product);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);

        if (product == null)
            return NotFound();

        _mapper.Map(updateProductDto, product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchProduct(int id,
        [FromBody] JsonPatchDocument<UpdateProductDto> patch)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);
        if (product == null)
            return NotFound();
        
        var productToUpdateDto = _mapper.Map<UpdateProductDto>(product);
        
        patch.ApplyTo(productToUpdateDto, ModelState);

        if (!TryValidateModel(productToUpdateDto))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(productToUpdateDto, product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);

        if (product == null)
            return NotFound();
        
        _context.Remove<Product>(product);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}