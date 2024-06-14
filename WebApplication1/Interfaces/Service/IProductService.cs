using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Data.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Interfaces.Service;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts(int take, int skip = 0);
    Task<Product?> GetProductById(int id);
    Task<EntityEntry<Product>> AddProduct([FromBody] CreateProductDto productDto);
    Task UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto);
    Task PatchProduct(int id, JsonPatchDocument<UpdateProductDto> patch);
    Task DeleteProduct(int id);
}