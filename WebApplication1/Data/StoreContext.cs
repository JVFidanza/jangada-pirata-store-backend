using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> opts) : base(opts)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
}