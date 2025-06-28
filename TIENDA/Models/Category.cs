using System.ComponentModel.DataAnnotations;

namespace TIENDA.Models;

public class Category
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public string Color { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public string Image { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int ProductCount { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();




}
