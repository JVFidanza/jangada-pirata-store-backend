using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Data;
using WebApplication1.Data.Dtos;
using WebApplication1.Interfaces.Service;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class ProductService : IProductService
{
    
    private StoreContext _context;
    private IMapper _mapper;

    public ProductService(StoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public IEnumerable<Product> GetAllProducts(int take, int skip = 0)
    {
        var productList = _context.Products;
        
        return take != default ? productList.Skip(skip).Take(take) : productList.Skip(skip);
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
            throw new ArgumentException("Product not found");
        
        return product;
    }

    public async Task<EntityEntry<Product>> AddProduct(CreateProductDto productDto)
    {
        Product product = _mapper.Map<Product>(productDto); 
        
        var createdProduct = await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return createdProduct;
    }

    public async Task UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);

        if (product == null)
            throw new ArgumentException("Product not found");

        _mapper.Map(updateProductDto, product);
        await _context.SaveChangesAsync();
    }

    public async Task PatchProduct(int id, JsonPatchDocument<UpdateProductDto> patch)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);
        
        if (product == null)
            throw new ArgumentException("Product not found");
        
        var productToUpdateDto = _mapper.Map<UpdateProductDto>(product);

        ModelStateDictionary modelState = new ModelStateDictionary();
        
        patch.ApplyTo(productToUpdateDto, modelState);

        if (!modelState.IsValid)
        {
            var nameOfClass = nameof(UpdateProductDto);
            var errorList = modelState[nameOfClass]?.Errors.Select(er => er.ErrorMessage);
            throw new ValidationException(String.Join(" || ", errorList.ToArray()));
        }

        _mapper.Map(productToUpdateDto, product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == id);

        if (product == null)
            throw new ArgumentException("Product not found");
        
        _context.Remove<Product>(product);
        await _context.SaveChangesAsync();
    }
}