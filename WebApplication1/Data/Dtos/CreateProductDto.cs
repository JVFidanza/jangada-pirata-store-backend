using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Dtos;

public class CreateProductDto
{
    [Required]
    public string ProductName { get; set; }
    public string ProductType { get; set; }
    public float ProductPrice { get; set; }
}