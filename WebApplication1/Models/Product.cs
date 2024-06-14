using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string ProductName { get; set; }
    public string ProductType { get; set; }
    public float ProductPrice { get; set; }
}