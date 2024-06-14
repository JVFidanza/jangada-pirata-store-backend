using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> opts) : base(opts)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
}